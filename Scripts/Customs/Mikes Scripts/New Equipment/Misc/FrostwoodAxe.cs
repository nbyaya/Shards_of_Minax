using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    public class FrostwoodAxe : Item
    {
        public override string DefaultName
        {
            get { return "a frostwood axe"; }
        }

        private int m_Uses;

        [Constructable]
        public FrostwoodAxe() : base(0xF43) // Axe item ID
        {
            Weight = 5.0;
            Hue = 0x44B; // Unique color for the axe
            m_Uses = 50; // Total uses
        }

        public FrostwoodAxe(Serial serial) : base(serial)
        {
        }

        // Oak tree tiles (fill this array with the tile IDs for oak trees)
        private static int[] m_OakTreeTiles = new int[]
        {
            // Example tile IDs for oak trees
				0x4CCA, 0x4CCB, 0x4CCC, 0x4CCD, 0x4CD0, 0x4CD3, 0x4CD6, 0x4CD8,
				0x4CDA, 0x4CDD, 0x4CE0, 0x4CE3, 0x4CE6, 0x4CF8, 0x4CFB, 0x4CFE,
				0x4D01, 0x4D41, 0x4D42, 0x4D43, 0x4D44, 0x4D57, 0x4D58, 0x4D59,
				0x4D5A, 0x4D5B, 0x4D6E, 0x4D6F, 0x4D70, 0x4D71, 0x4D72, 0x4D84,
				0x4D85, 0x4D86, 0x52B5, 0x52B6, 0x52B7, 0x52B8, 0x52B9, 0x52BA,
				0x52BB, 0x52BC, 0x52BD,

				0x4CCE, 0x4CCF, 0x4CD1, 0x4CD2, 0x4CD4, 0x4CD5, 0x4CD7, 0x4CD9,
				0x4CDB, 0x4CDC, 0x4CDE, 0x4CDF, 0x4CE1, 0x4CE2, 0x4CE4, 0x4CE5,
				0x4CE7, 0x4CE8, 0x4CF9, 0x4CFA, 0x4CFC, 0x4CFD, 0x4CFF, 0x4D00,
				0x4D02, 0x4D03, 0x4D45, 0x4D46, 0x4D47, 0x4D48, 0x4D49, 0x4D4A,
				0x4D4B, 0x4D4C, 0x4D4D, 0x4D4E, 0x4D4F, 0x4D50, 0x4D51, 0x4D52,
				0x4D53, 0x4D5C, 0x4D5D, 0x4D5E, 0x4D5F, 0x4D60, 0x4D61, 0x4D62,
				0x4D63, 0x4D64, 0x4D65, 0x4D66, 0x4D67, 0x4D68, 0x4D69, 0x4D73,
				0x4D74, 0x4D75, 0x4D76, 0x4D77, 0x4D78, 0x4D79, 0x4D7A, 0x4D7B,
				0x4D7C, 0x4D7D, 0x4D7E, 0x4D7F, 0x4D87, 0x4D88, 0x4D89, 0x4D8A,
				0x4D8B, 0x4D8C, 0x4D8D, 0x4D8E, 0x4D8F, 0x4D90, 0x4D95, 0x4D96,
				0x4D97, 0x4D99, 0x4D9A, 0x4D9B, 0x4D9D, 0x4D9E, 0x4D9F, 0x4DA1,
				0x4DA2, 0x4DA3, 0x4DA5, 0x4DA6, 0x4DA7, 0x4DA9, 0x4DAA, 0x4DAB,
				0x52BE, 0x52BF, 0x52C0, 0x52C1, 0x52C2, 0x52C3, 0x52C4, 0x52C5,
				0x52C6, 0x52C7
        };

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Skills.Lumberjacking.Value < 100)
            {
                from.SendMessage("You need at least 100 lumberjacking skill to use this axe.");
                return;
            }

            if (m_Uses > 0)
            {
                from.BeginTarget(-1, true, TargetFlags.None, new TargetCallback(OnTarget));
            }
            else
            {
                from.SendMessage("This axe has worn out.");
                this.Delete();
            }
        }

        private void OnTarget(Mobile from, object targeted)
        {
            if (targeted is StaticTarget)
            {
                StaticTarget target = (StaticTarget)targeted;

                if (Array.IndexOf(m_OakTreeTiles, target.ItemID) >= 0)
                {
                    from.AddToBackpack(new FrostwoodLog()); // OakLog item to be created
                    m_Uses--;

                    if (m_Uses <= 0)
                    {
                        from.SendMessage("The special axe breaks!");
                        this.Delete();
                    }
                }
                else
                {
                    from.AddToBackpack(new FrostwoodLog()); // OakLog item to be created
                    m_Uses--;
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Uses);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Uses = reader.ReadInt();
        }
    }
}
