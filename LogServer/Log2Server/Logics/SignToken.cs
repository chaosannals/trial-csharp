using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log2Server.Logics;

public class SignToken
{
    public long AppId { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime ExpireAt { get; set; }
}
