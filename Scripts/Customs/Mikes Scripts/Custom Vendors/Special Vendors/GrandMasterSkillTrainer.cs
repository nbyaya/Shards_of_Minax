using Server.Mobiles;
using Server.Items; // Required for handling items such as MaxxiaScroll
using Server.Gumps;

namespace Server.Custom.SpecialVendor
{
    public class GrandMasterSkillTrainer : BaseCreature
    {
        [Constructable]
        public GrandMasterSkillTrainer() : base(AIType.AI_Vendor, FightMode.None, 2, 1, 0.5, 2)
        {
            // Set the NPC's attributes here
            Hue = Utility.RandomSkinHue();
            Body = 0x190;
            Name = "Grandmaster Trainer";

            // Abilities and stats
            SetStr(300, 400);
            SetDex(70, 95);
            SetInt(170, 220);
            SetHits(260, 310);

            VirtualArmor = 30;

            SpeechHue = Utility.RandomDyedHue();
            SetDamage(10, 14);

            SetSkill(SkillName.MagicResist, 75.1, 95.0);
            SetSkill(SkillName.Tactics, 70.1, 95.0);
            SetSkill(SkillName.Magery, 80.0, 95.0);
			this.AddItem(new Sandals());				
            this.AddItem(new FancyShirt(0x6CC));				
            this.AddItem(new LongPants(0x5E8));	
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 4))
            {
                if (from is PlayerMobile)
                {
                    PlayerMobile pm = (PlayerMobile)from;
                    from.CloseGump(typeof(PowerscrollGMVendorGump));
                    from.SendGump(new PowerscrollGMVendorGump(pm, 0));
                }
            }
            else
            {
                from.SendMessage("You are too far away.");
            }
        }

        public GrandMasterSkillTrainer(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
