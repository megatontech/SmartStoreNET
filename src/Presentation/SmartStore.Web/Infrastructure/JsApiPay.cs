using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization;
using System.IO;
using System.Text;
using System.Net;
using System.Web.Security;
using LitJson;

namespace SmartStore.Web.Infrastructure
{
    public class JsApiPay
    {
        /// <summary>
        /// 保存页面对象，因为要在类的方法中使用Page的Request对象
        /// </summary>
        private HttpRequestBase page {get;set;}

        /// <summary>
        /// openid用于调用统一下单接口
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// access_token用于获取收货地址js函数入口参数
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 商品金额，用于统一下单
        /// </summary>
        public int total_fee { get; set; }

        /// <summary>
        /// 统一下单接口返回结果
        /// </summary>
        public WxPayData unifiedOrderResult { get; set; } 

        public JsApiPay(HttpRequestBase page)
        {
            this.page = page;
        }


        /**
        * 
        * 网页授权获取用户基本信息的全部过程
        * 详情请参看网页授权获取用户基本信息：http://mp.weixin.qq.com/wiki/17/c0f37d5704f0b64713d5d2c37b468d75.html
        * 第一步：利用url跳转获取code
        * 第二步：利用code去获取openid和access_token
        * 
        */
        public void GetOpenidAndAccessToken()
        {
            if (!string.IsNullOrEmpty(page.QueryString["code"]))
            {
                //获取code码，以获取openid和access_token
                string code = page.QueryString["code"];
                Log.Debug(this.GetType().ToString(), "Get code : " + code);
                GetOpenidAndAccessTokenFromCode(code);
            }
            else
            {
                //构造网页授权获取code的URL
                string host = page.Url.Host;
                string path = page.Path;
                string redirect_uri = HttpUtility.UrlEncode("http://" + host + path);
                WxPayData data = new WxPayData();
                data.SetValue("appid", WxPayConfig.GetConfig().GetAppID());
                data.SetValue("redirect_uri", redirect_uri);
                data.SetValue("response_type", "code");
                data.SetValue("scope", "snsapi_base");
                data.SetValue("state", "STATE" + "#wechat_redirect");
                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?" + data.ToUrl();
                Log.Debug(this.GetType().ToString(), "Will Redirect to URL : " + url);
                try
                {
                    //触发微信返回code码         
                   // Response.Redirect(url);//Redirect函数会抛出ThreadAbortException异常，不用处理这个异常
                }
                catch(System.Threading.ThreadAbortException ex)
                {
                }
            }
        }


        /**
	    * 
	    * 通过code换取网页授权access_token和openid的返回数据，正确时返回的JSON数据包如下：
	    * {
	    *  "access_token":"ACCESS_TOKEN",
	    *  "expires_in":7200,
	    *  "refresh_token":"REFRESH_TOKEN",
	    *  "openid":"OPENID",
	    *  "scope":"SCOPE",
	    *  "unionid": "o6_bmasdasdsad6_2sgVt7hMZOPfL"
	    * }
	    * 其中access_token可用于获取共享收货地址
	    * openid是微信支付jsapi支付接口统一下单时必须的参数
        * 更详细的说明请参考网页授权获取用户基本信息：http://mp.weixin.qq.com/wiki/17/c0f37d5704f0b64713d5d2c37b468d75.html
        * @失败时抛异常WxPayException
	    */
        public void GetOpenidAndAccessTokenFromCode(string code)
        {
            try
            {
                //构造获取openid及access_token的url
                WxPayData data = new WxPayData();
                data.SetValue("appid", WxPayConfig.GetConfig().GetAppID());
                data.SetValue("secret", WxPayConfig.GetConfig().GetAppSecret());
                data.SetValue("code", code);
                data.SetValue("grant_type", "authorization_code");
                string url = "https://api.weixin.qq.com/sns/oauth2/access_token?" + data.ToUrl();

                //请求url以获取数据
                string result = HttpService.Get(url);

                Log.Debug(this.GetType().ToString(), "GetOpenidAndAccessTokenFromCode response : " + result);

                //保存access_token，用于收货地址获取
                JsonData jd = JsonMapper.ToObject(result);
                access_token = (string)jd["access_token"];

                //获取用户openid
                openid = (string)jd["openid"];

                Log.Debug(this.GetType().ToString(), "Get openid : " + openid);
                Log.Debug(this.GetType().ToString(), "Get access_token : " + access_token);
            }
            catch (Exception ex)
            {
                Log.Error(this.GetType().ToString(), ex.ToString());
                throw new WxPayException(ex.ToString());
            }
        }

        /**
         * 调用统一下单，获得下单结果
         * @return 统一下单结果
         * @失败时抛异常WxPayException
         */
        public WxPayData GetUnifiedOrderResult(string product)
        {
            //统一下单
            WxPayData data = new WxPayData();
            data.SetValue("body", product);
            //data.SetValue("attach", "test");
            data.SetValue("out_trade_no", WxPayApi.GenerateOutTradeNo());
            data.SetValue("total_fee", total_fee);
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            data.SetValue("time_expire", DateTime.Now.AddMinutes(30).ToString("yyyyMMddHHmmss"));
            //data.SetValue("goods_tag", "test");
            data.SetValue("trade_type", "JSAPI");
            data.SetValue("openid", openid);

            WxPayData result = WxPayApi.UnifiedOrder(data);
            if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
            {
                Log.Error(this.GetType().ToString(), "UnifiedOrder response error!");
                throw new WxPayException("UnifiedOrder response error!");
            }

            unifiedOrderResult = result;
            return result;
        }

        /**
        *  
        * 从统一下单成功返回的数据中获取微信浏览器调起jsapi支付所需的参数，
        * 微信浏览器调起JSAPI时的输入参数格式如下：
        * {
        *   "appId" : "wx2421b1c4370ec43b",     //公众号名称，由商户传入     
        *   "timeStamp":" 1395712654",         //时间戳，自1970年以来的秒数     
        *   "nonceStr" : "e61463f8efa94090b1f366cccfbbb444", //随机串     
        *   "package" : "prepay_id=u802345jgfjsdfgsdg888",     
        *   "signType" : "MD5",         //微信签名方式:    
        *   "paySign" : "70EA570631E4BB79628FBCA90534C63FF7FADD89" //微信签名 
        * }
        * @return string 微信浏览器调起JSAPI时的输入参数，json格式可以直接做参数用
        * 更详细的说明请参考网页端调起支付API：http://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_7
        * 
        */
        public string GetJsApiParameters()
        {
            Log.Debug(this.GetType().ToString(), "JsApiPay::GetJsApiParam is processing...");

            WxPayData jsApiParam = new WxPayData();
            jsApiParam.SetValue("appId", unifiedOrderResult.GetValue("appid"));
            jsApiParam.SetValue("timeStamp", WxPayApi.GenerateTimeStamp());
            jsApiParam.SetValue("nonceStr", WxPayApi.GenerateNonceStr());
            jsApiParam.SetValue("package", "prepay_id=" + unifiedOrderResult.GetValue("prepay_id"));
            jsApiParam.SetValue("signType", "MD5");
            jsApiParam.SetValue("paySign", jsApiParam.MakeSign());

            string parameters = jsApiParam.ToJson();

            Log.Debug(this.GetType().ToString(), "Get jsApiParam : " + parameters);
            return parameters;
        }


        /**
	    * 
	    * 获取收货地址js函数入口参数,详情请参考收货地址共享接口：http://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_9
	    * @return string 共享收货地址js函数需要的参数，json格式可以直接做参数使用
	    */
        public string GetEditAddressParameters()
	    {
            string parameter = "";
            try
            {
                string host = page.Url.Host;
                string path = page.Path;
                string queryString = page.Url.Query;
                //这个地方要注意，参与签名的是网页授权获取用户信息时微信后台回传的完整url
                string url = "http://" + host + path + queryString;

                //构造需要用SHA1算法加密的数据
                WxPayData signData = new WxPayData();
                signData.SetValue("appid",WxPayConfig.GetConfig().GetAppID());
                signData.SetValue("url", url);
                signData.SetValue("timestamp",WxPayApi.GenerateTimeStamp());
                signData.SetValue("noncestr",WxPayApi.GenerateNonceStr());
                signData.SetValue("accesstoken",access_token);
                string param = signData.ToUrl();

                Log.Debug(this.GetType().ToString(), "SHA1 encrypt param : " + param);
                //SHA1加密
                string addrSign = FormsAuthentication.HashPasswordForStoringInConfigFile(param, "SHA1");
                Log.Debug(this.GetType().ToString(), "SHA1 encrypt result : " + addrSign);

                //获取收货地址js函数入口参数
                WxPayData afterData = new WxPayData();
                afterData.SetValue("appId",WxPayConfig.GetConfig().GetAppID());
                afterData.SetValue("scope","jsapi_address");
                afterData.SetValue("signType","sha1");
                afterData.SetValue("addrSign",addrSign);
                afterData.SetValue("timeStamp",signData.GetValue("timestamp"));
                afterData.SetValue("nonceStr",signData.GetValue("noncestr"));

                //转为json格式
                parameter = afterData.ToJson();
                Log.Debug(this.GetType().ToString(), "Get EditAddressParam : " + parameter);
            }
            catch (Exception ex)
            {
                Log.Error(this.GetType().ToString(), ex.ToString());
                throw new WxPayException(ex.ToString());
            }

            return parameter;
	    }

//        接口链接
//URL地址：https://api.mch.weixin.qq.com/pay/unifiedorder

//URL地址：https://api2.mch.weixin.qq.com/pay/unifiedorder(备用域名)见跨城冗灾方案

//是否需要证书
//否

//请求参数
//字段名 变量名 必填  类型 示例值 描述
//公众账号ID  appid 是   String(32)  wxd678efh567hg6787 微信支付分配的公众账号ID（企业号corpid即为此appId）
//商户号 mch_id  是 String(32)  1230000109	微信支付分配的商户号
//设备号 device_info 否   String(32)  013467007045764	自定义参数，可以为终端设备号(门店号或收银设备ID)，PC网页或公众号内支付可以传"WEB"
//随机字符串 nonce_str   是 String(32)  5K8264ILTKCH16CQ2502SI8ZNMTM67VS 随机字符串，长度要求在32位以内。推荐随机数生成算法
//签名  sign 是   String(32)  C380BEC2BFD727A4B6845133519F3AD6 通过签名算法计算得出的签名值，详见签名生成算法
//签名类型    sign_type 否   String(32)  MD5 签名类型，默认为MD5，支持HMAC-SHA256和MD5。
//商品描述 body    是 String(128) 腾讯充值中心-QQ会员充值
//商品简单描述，该字段请按照规范传递，具体请见参数规定

//商品详情    detail 否   String(6000)        商品详细描述，对于使用单品优惠的商户，该字段必须按照规范上传，详见“单品优惠参数说明”
//附加数据 attach  否 String(127) 深圳分店 附加数据，在查询API和支付通知中原样返回，可作为自定义参数使用。
//商户订单号 out_trade_no    是 String(32)  20150806125346	商户系统内部订单号，要求32个字符内，只能是数字、大小写字母_-|* 且在同一个商户号下唯一。详见商户订单号
//标价币种    fee_type 否   String(16)  CNY 符合ISO 4217标准的三位字母代码，默认人民币：CNY，详细列表请参见货币类型
//标价金额    total_fee 是   Int	88	订单总金额，单位为分，详见支付金额
//终端IP    spbill_create_ip 是   String(64)  123.12.12.123	支持IPV4和IPV6两种格式的IP地址。用户的客户端IP
//交易起始时间  time_start 否   String(14)  20091225091010	订单生成时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010。其他详见时间规则
//交易结束时间  time_expire 否   String(14)  20091227091010	
//订单失效时间，格式为yyyyMMddHHmmss，如2009年12月27日9点10分10秒表示为20091227091010。订单失效时间是针对订单号而言的，由于在请求支付的时候有一个必传参数prepay_id只有两小时的有效期，所以在重入时间超过2小时的时候需要重新请求下单接口获取新的prepay_id。其他详见时间规则

//time_expire只能第一次下单传值，不允许二次修改，二次修改系统将报错。如用户支付失败后，需再次支付，需更换原订单号重新下单。
//建议：最短失效时间间隔大于1分钟

//订单优惠标记  goods_tag 否   String(32)  WXG 订单优惠标记，使用代金券或立减优惠功能时需要的参数，说明详见代金券或立减优惠
//通知地址    notify_url 是   String(256) http://www.weixin.qq.com/wxpay/pay.php	异步接收微信支付结果通知的回调地址，通知url必须为外网可访问的url，不能携带参数。
//交易类型 trade_type  是 String(16)  JSAPI
//JSAPI -JSAPI支付

//NATIVE -Native支付

//APP -APP支付

//说明详见参数规定

//商品ID product_id  否 String(32)  12235413214070356458058	trade_type=NATIVE时，此参数必传。此参数为二维码中包含的商品ID，商户自行定义。
//指定支付方式 limit_pay   否 String(32)  no_credit 上传此参数no_credit--可限制用户不能使用信用卡支付
//用户标识    openid 否   String(128) oUpF8uMuAJO_M2pxb1Q9zNjWeS6o trade_type = JSAPI时（即JSAPI支付），此参数必传，此参数为微信用户在商户对应appid下的唯一标识。openid如何获取，可参考【获取openid】。企业号请使用【企业号OAuth2.0接口】获取企业号内成员userid，再调用【企业号userid转openid接口】进行转换
//电子发票入口开放标识  receipt 否   String(8)   Y Y，传入Y时，支付成功消息和支付详情页将出现开票入口。需要在微信支付商户平台或微信公众平台开通电子发票功能，传此字段才可生效
//+场景信息 scene_info  否 String(256)
//        {
//            "store_info" : {
//                "id": "SZTX001",
//"name": "腾大餐厅",
//"area_code": "440305",
//"address": "科技园中一路腾讯大厦" }
//        }

//        该字段常用于线下活动时的场景信息上报，支持上报实际门店信息，商户也可以按需求自己上报相关信息。该字段为JSON对象数据，对象格式为{"store_info":{"id": "门店ID","name": "名称","area_code": "编码","address": "地址" }} ，字段详细说明请点击行前的+展开

//举例如下：

//<xml>
//   <appid>wx2421b1c4370ec43b</appid>
//   <attach>支付测试</attach>
//   <body>JSAPI支付测试</body>
//   <mch_id>10000100</mch_id>
//   <detail><![CDATA[{ "goods_detail":[ { "goods_id":"iphone6s_16G", "wxpay_goods_id":"1001", "goods_name":"iPhone6s 16G", "quantity":1, "price":528800, "goods_category":"123456", "body":"苹果手机" }, { "goods_id":"iphone6s_32G", "wxpay_goods_id":"1002", "goods_name":"iPhone6s 32G", "quantity":1, "price":608800, "goods_category":"123789", "body":"苹果手机" } ] }]]></detail>
//   <nonce_str>1add1a30ac87aa2db72f57a2375d8fec</nonce_str>
//   <notify_url>http://wxpay.wxutil.com/pub_v2/pay/notify.v2.php</notify_url>
//   <openid>oUpF8uMuAJO_M2pxb1Q9zNjWeS6o</openid>
//   <out_trade_no>1415659990</out_trade_no>
//   <spbill_create_ip>14.23.150.211</spbill_create_ip>
//   <total_fee>1</total_fee>
//   <trade_type>JSAPI</trade_type>
//   <sign>0CB01533B8C1EF103065174F50BCA001</sign>
//</xml>

//注：参数值用XML转义即可，CDATA标签用于说明数据不被XML解析器解析。

//返回结果
//字段名 变量名 必填  类型 示例值 描述
//返回状态码   return_code 是   String(16)  SUCCESS
//SUCCESS/FAIL

//此字段是通信标识，非交易标识，交易是否成功需要查看result_code来判断

//返回信息    return_msg 是   String(128) OK
//当return_code为FAIL时返回信息为错误原因 ，例如

//签名失败

//参数格式校验错误

//以下字段在return_code为SUCCESS的时候有返回

//字段名 变量名 必填 类型  示例值 描述
//公众账号ID appid   是 String(32)  wx8888888888888888 调用接口提交的公众账号ID
//商户号 mch_id  是 String(32)  1900000109	调用接口提交的商户号
//设备号 device_info 否   String(32)  013467007045764	自定义参数，可以为请求支付的终端设备号等
//随机字符串   nonce_str 是   String(32)  5K8264ILTKCH16CQ2502SI8ZNMTM67VS 微信返回的随机字符串
//签名 sign    是 String(32)  C380BEC2BFD727A4B6845133519F3AD6 微信返回的签名值，详见签名算法
//业务结果    result_code 是   String(16)  SUCCESS SUCCESS/FAIL
//错误代码    err_code 否   String(32)      当result_code为FAIL时返回错误代码，详细参见下文错误列表
//错误代码描述  err_code_des 否   String(128)     当result_code为FAIL时返回错误描述，详细参见下文错误列表
//以下字段在return_code 和result_code都为SUCCESS的时候有返回

//字段名 变量名 必填  类型 示例值 描述
//交易类型    trade_type 是   String(16)  JSAPI
//JSAPI -JSAPI支付

//NATIVE -Native支付

//APP -APP支付

//说明详见参数规定

//预支付交易会话标识 prepay_id   是 String(64)  wx201410272009395522657a690389285100 微信生成的预支付会话标识，用于后续接口调用中使用，该值有效期为2小时
//二维码链接   code_url 否   String(64)  weixin://wxpay/bizpayurl/up?pr=NwY5Mz9&groupid=00	
//trade_type=NATIVE时有返回，此url用于生成支付二维码，然后提供给用户进行扫码支付。

//注意：code_url的值并非固定，使用时按照URL格式转成二维码即可

//举例如下：

//<xml>
//   <return_code><![CDATA[SUCCESS]]></return_code>
//   <return_msg><![CDATA[OK]]></return_msg>
//   <appid><![CDATA[wx2421b1c4370ec43b]]></appid>
//   <mch_id><![CDATA[10000100]]></mch_id>
//   <nonce_str><![CDATA[IITRi8Iabbblz1Jc]]></nonce_str>
//   <openid><![CDATA[oUpF8uMuAJO_M2pxb1Q9zNjWeS6o]]></openid>
//   <sign><![CDATA[7921E432F65EB8ED0CE9755F0E86D72F]]></sign>
//   <result_code><![CDATA[SUCCESS]]></result_code>
//   <prepay_id><![CDATA[wx201411101639507cbf6ffd8b0779950874]]></prepay_id>
//   <trade_type><![CDATA[JSAPI]]></trade_type>
//</xml>

//错误码
//名称  描述 原因  解决方案
//INVALID_REQUEST 参数错误 参数格式有误或者未按规则上传  订单重入时，要求参数值与原请求一致，请确认参数问题
//NOAUTH  商户无此接口权限 商户未开通此接口权限  请商户前往申请此接口权限
//NOTENOUGH   余额不足 用户帐号余额不足    用户帐号余额不足，请用户充值或更换支付卡后再支付
//ORDERPAID   商户订单已支付 商户订单已支付，无需重复操作 商户订单已支付，无需更多操作
//ORDERCLOSED 订单已关闭 当前订单已关闭，无法支付 当前订单已关闭，请重新下单
//SYSTEMERROR 系统错误 系统超时    系统异常，请用相同参数重新调用
//APPID_NOT_EXIST APPID不存在 参数中缺少APPID  请检查APPID是否正确
//MCHID_NOT_EXIST MCHID不存在 参数中缺少MCHID  请检查MCHID是否正确
//APPID_MCHID_NOT_MATCH   appid和mch_id不匹配 appid和mch_id不匹配 请确认appid和mch_id是否匹配
//LACK_PARAMS 缺少参数 缺少必要的请求参数   请检查参数是否齐全
//OUT_TRADE_NO_USED   商户订单号重复 同一笔交易不能多次提交 请核实商户订单号是否重复提交
//SIGNERROR   签名错误 参数签名结果不正确   请检查签名参数和方法是否都符合签名算法要求
//XML_FORMAT_ERROR    XML格式错误 XML格式错误 请检查XML参数格式是否正确
//REQUIRE_POST_METHOD 请使用post方法 未使用post传递参数     请检查请求参数是否通过post方法提交
//POST_DATA_EMPTY post数据为空 post数据不能为空  请检查post数据是否为空
//NOT_UTF8    编码格式错误 未使用指定编码格式   请使用UTF-8编码格式
    }
}