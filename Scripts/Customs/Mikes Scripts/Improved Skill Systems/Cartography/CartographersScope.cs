using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Regions;

namespace Server.Items
{
    public class CartographersScope : Item
    {
        private DateTime m_NextUse;
        private Point3D m_LastLocation;

        [Constructable]
        public CartographersScope() : base(0x14F5)
        {
            Name = "Cartographer's Scope";
            Weight = 1.0;
        }

        public CartographersScope(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile))
                return;

            PlayerMobile pm = (PlayerMobile)from;

            if (DateTime.Now < m_NextUse)
            {
                pm.SendMessage("You must wait before using the scope again.");
                return;
            }

            int distanceThreshold = 40;
            if (m_LastLocation != Point3D.Zero && 
                !HasMovedFarEnough(pm.Location, m_LastLocation, distanceThreshold))
            {
                pm.SendMessage("You haven't moved far enough to use the scope.");
                return;
            }

            double cartographySkill = pm.Skills[SkillName.Cartography].Value;

            // Calculate cooldown based on Cartography skill
            int cooldownSeconds = Math.Max(1, 10 - (int)(cartographySkill / 20));
            m_NextUse = DateTime.Now.AddSeconds(cooldownSeconds);

            // Determine report type based on location and Cartography skill
            Item report = CreateReport(pm);

            if (pm.AddToBackpack(report))
            {
                pm.SendMessage("You use the scope and create a scouting report.");
                m_LastLocation = pm.Location;
            }
            else
            {
                pm.SendMessage("Your backpack is too full to hold the scouting report.");
                report.Delete();
            }
        }

        private Item CreateReport(PlayerMobile pm)
        {
            double rand = Utility.RandomDouble();
            double cartographySkill = pm.Skills[SkillName.Cartography].Value;

            if (rand < 0.01 + (cartographySkill / 2000)) // 1% base chance + up to 5% based on skill
                return new ExceptionalReport();
            else if (rand < 0.05 + (cartographySkill / 1000)) // 5% base chance + up to 10% based on skill
                return new AdvancedReport();

            // Check location and create special reports
            Region region = pm.Region;
            Map map = pm.Map;

            if (region is TownRegion)
                return new UrbanReport();
			else if (map == Map.TerMur)
                return new TerMurReport();
            else if (region is DungeonRegion)
                return new ArchaeologicalReport();
            else if (map == Map.Ilshenar)
                return new IlshenarReport();
            else if (map == Map.Malas)
                return new MalasReport();
            else if (map == Map.Tokuno)
                return new TokunoReport();
            else if (map == Map.Felucca || map == Map.Trammel)
                return new ScoutingReport();

            // Default to regular scouting report if no special conditions are met
            return new ScoutingReport();
        }

        private bool HasMovedFarEnough(Point3D current, Point3D last, int threshold)
        {
            int xDiff = current.X - last.X;
            int yDiff = current.Y - last.Y;
            int zDiff = current.Z - last.Z;

            return ((xDiff * xDiff) + (yDiff * yDiff) + (zDiff * zDiff)) >= (threshold * threshold);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(m_NextUse);
            writer.Write(m_LastLocation);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextUse = reader.ReadDateTime();
            m_LastLocation = reader.ReadPoint3D();
        }
    }

    public class ScoutingReport : Item
    {
        [Constructable]
        public ScoutingReport() : base(0x14ED)
        {
            Name = "Scouting Report";
            Weight = 0.1;
            Hue = 2225;
        }

        public ScoutingReport(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class AdvancedReport : ScoutingReport
    {
        [Constructable]
        public AdvancedReport() : base()
        {
            Name = "Advanced Scouting Report";
            Hue = 1125;
        }

        public AdvancedReport(Serial serial) : base(serial)
        {
        }
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class ExceptionalReport : ScoutingReport
    {
        [Constructable]
        public ExceptionalReport() : base()
        {
            Name = "Exceptional Scouting Report";
            Hue = 425;
        }

        public ExceptionalReport(Serial serial) : base(serial)
        {
        }
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class UrbanReport : ScoutingReport
    {
        [Constructable]
        public UrbanReport() : base()
        {
            Name = "Urban Scouting Report";
            Hue = 1641;
        }

        public UrbanReport(Serial serial) : base(serial)
        {
        }
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class ArchaeologicalReport : ScoutingReport
    {
        [Constructable]
        public ArchaeologicalReport() : base()
        {
            Name = "Archaeological Scouting Report";
            Hue = 2212;
        }

        public ArchaeologicalReport(Serial serial) : base(serial)
        {
        }
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class IlshenarReport : ScoutingReport
    {
        [Constructable]
        public IlshenarReport() : base()
        {
            Name = "Ilshenar Scouting Report";
            Hue = 1854;
        }

        public IlshenarReport(Serial serial) : base(serial)
        {
        }
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class MalasReport : ScoutingReport
    {
        [Constructable]
        public MalasReport() : base()
        {
            Name = "Malas Scouting Report";
            Hue = 1645;
        }

        public MalasReport(Serial serial) : base(serial)
        {
        }
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class TokunoReport : ScoutingReport
    {
        [Constructable]
        public TokunoReport() : base()
        {
            Name = "Tokuno Scouting Report";
            Hue = 1175;
        }

        public TokunoReport(Serial serial) : base(serial)
        {
        }
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class TerMurReport : ScoutingReport
    {
        [Constructable]
        public TerMurReport() : base()
        {
            Name = "Ter Mur Scouting Report";
            Hue = 2068;
        }

        public TerMurReport(Serial serial) : base(serial)
        {
        }
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}