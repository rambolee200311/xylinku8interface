using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace U8toOAInterface.Models.U8WriteFhData
{
    public class Detail1Data
    {
        /*
            子件编码	zjbm	varchar(1000)	detail1	是	是	传入：浏览框的数据标题保存的时候必填（如果是自动增长字段可以不填，否则必填），更新的时候必填
            子件名称	zjmc	varchar(200)	detail1	是	是	
            子件数量	zjsl	int(11)	detail1	是	是	
            含税单价	hsdj	decimal(38,2)	detail1	是	是	
            无税单价	wsdj	decimal(38,2)	detail1	是	是
         */
        public string zjbm { get; set; }//子件编码
        public string zjmc { get; set; }//子件编码
        public decimal zjsl { get; set; }//子件数量
        public decimal hsdj { get; set; }//含税单价
        public decimal wsdj { get; set; }//无税单价

    }
}
