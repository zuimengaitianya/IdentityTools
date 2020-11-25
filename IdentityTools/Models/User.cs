using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTools.Models
{
    public class UserInfo
    {
        //[Required(ErrorMessage = "不能为空")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required, Display()]
        [EmailAddress]
        [StringLength(16, ErrorMessage = "账号的长度为{1}个字符")]
        public string UserAccount { get; set; }

        [Range(0, 200, ErrorMessage = "年龄的范围在{1}至{2}之间")]
        public int UserAge { get; set; }

        public string UserName { get; set; }

        public string UserPhone { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UserCreateTime { get; set; }
    }

    public class UserRole
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserAccount { get; set; }

        public string UserRoles { get; set; }
    }
}
