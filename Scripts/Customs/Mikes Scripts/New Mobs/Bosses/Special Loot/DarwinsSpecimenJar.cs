using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class DarwinsSpecimenJar : Item
    {
        [Constructable]
        public DarwinsSpecimenJar() : base(0xE24)
        {
            Name = "Darwin's Specimen Jar";
            Hue = 0x47E;
            Weight = 1.0;
            LootType = LootType.Regular;
        }

        public DarwinsSpecimenJar(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.GetWorldLocation(), 1))
            {
                BaseCreature creature = null;

                switch (Utility.Random(3))
                {
                    case 0: creature = new DireWolf(); break;
                    case 1: creature = new HellHound(); break;
                    case 2: creature = new WhiteWyrm(); break;
                }

                if (creature != null)
                {
                    creature.Controlled = true;
                    creature.ControlMaster = from;
                    creature.IsBonded = true;
                    creature.MoveToWorld(from.Location, from.Map);
                    from.SendMessage("You have summoned a companion from Darwin's Specimen Jar!");
                }

                this.Delete();
            }
            else
            {
                from.SendMessage("You are too far away to use that.");
            }
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
