using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalApplication.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }

        public string RoomPurpose { get; set; }

        public int RoomNumber { get; set; }

        public int Floor { get; set; }

        public string Building { get; set; }
    }

    public class RoomDto

    {
        public int RoomId { get; set; }

        public string RoomPurpose { get; set; }

        public int RoomNumber { get; set; }

        public int Floor { get; set; }

        public string Building { get; set; }
    }
}