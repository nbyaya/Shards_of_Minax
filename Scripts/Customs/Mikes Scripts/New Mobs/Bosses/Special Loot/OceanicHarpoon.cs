using System;
using Server;
using Server.Targeting;

namespace Server.Items
{
    public class OceanicHarpoon : BaseSpear
    {
        [Constructable]
        public OceanicHarpoon() : base(0x8FFA)
        {
            Name = "Oceanic Harpoon";
            Hue = 0x4F2;
            Weight = 8.0;
            
            WeaponAttributes.HitLeechMana = 20;
            Attributes.WeaponSpeed = 20;
            Attributes.WeaponDamage = 35;
            
            SkillBonuses.SetValues(0, SkillName.Fishing, 10.0);
        }

        public OceanicHarpoon(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Skills[SkillName.Fishing].Base < 80.0)
            {
                from.SendMessage("You need at least 80 fishing skill to use this harpoon effectively.");
                return;
            }

            from.Target = new InternalTarget(this);
            from.SendMessage("Select a fishing spot to use the Oceanic Harpoon.");
        }

        private class InternalTarget : Target
        {
            private OceanicHarpoon m_Harpoon;

            public InternalTarget(OceanicHarpoon harpoon) : base(10, true, TargetFlags.None)
            {
                m_Harpoon = harpoon;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Static && ((Static)targeted).ItemID >= 0x1796 && ((Static)targeted).ItemID <= 0x17B2)
                {
                    if (from.Skills[SkillName.Fishing].Value >= Utility.RandomMinMax(80, 100))
                    {
                        from.SendMessage("You catch a large fish with the Oceanic Harpoon!");
                        from.AddToBackpack(new BigFish());
                    }
                    else
                    {
                        from.SendMessage("You fail to catch anything with the Oceanic Harpoon.");
                    }
                }
                else
                {
                    from.SendMessage("You can only use this on fishing spots.");
                }
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