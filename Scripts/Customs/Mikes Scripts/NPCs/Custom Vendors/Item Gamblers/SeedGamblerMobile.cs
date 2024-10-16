using Server.Mobiles;
using Server.Items; // Required for handling items such as MaxxiaScroll

namespace Server.Custom.SpecialVendor
{
    public class SeedGamblerMobile : BaseCreature
    {
        // Existing code...
        [Constructable]
        public SeedGamblerMobile() : base(AIType.AI_Vendor, FightMode.None, 2, 1, 0.5, 2)
        {
            // Set the NPC's attributes here
            Hue = Utility.RandomSkinHue();
            Body = 0x190;
            Name = "Item Gambler";

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
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.Location, 4))
            {
                if (HasEnoughMaxxiaScroll(from, 1))
                {
                    DeductMaxxiaScroll(from, 1);
                    this.Say("Thank you for the Maxxia Scroll, here is the offer.");
                    from.SendGump(new SeedGamblerGump(from));
                }
                else
                {
                    this.Say("You do not have enough Maxxia Scrolls to gamble today.");
                }
            }
            else
            {
                from.SendMessage("You are too far away.");
            }
        }

        private bool HasEnoughMaxxiaScroll(Mobile from, int amount)
        {
            // Check the player's backpack for enough MaxxiaScrolls
            return from.Backpack != null && from.Backpack.FindItemByType(typeof(MaxxiaScroll)) != null;
        }

        private void DeductMaxxiaScroll(Mobile from, int amount)
        {
            // Try to deduct from the backpack
            if (from.Backpack != null)
            {
                Item item = from.Backpack.FindItemByType(typeof(MaxxiaScroll));
                if (item != null)
                    item.Consume(amount);
            }
        }

        // ... The rest of your existing code...
        public SeedGamblerMobile(Serial serial) : base(serial)
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
