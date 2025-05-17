using System;

namespace Server.Items
{
    [FlipableAttribute(0x13B2, 0x13B1)]
    public class JukaBow : Bow
    {
        [Constructable]
        public JukaBow()
        {
            ApplyRandomTier();
        }

        public JukaBow(Serial serial)
            : base(serial)
        { }

        private void ApplyRandomTier()
        {
            Random rand = new Random();
            double chanceForSpecialTier = rand.NextDouble();

            // 50% chance for default stats, 50% chance for a special tier
            if (chanceForSpecialTier < 0.5)
            {
                // Default stats, don't modify anything
                return;
            }

            // Now determine which special tier it falls into
            double tierChance = rand.NextDouble();

            if (tierChance < 0.05)
            {
                this.MinDamage = rand.Next(1, 80);
                this.MaxDamage = rand.Next(80, 120);
            }
            else if (tierChance < 0.2)
            {
                this.MinDamage = rand.Next(1, 70);
                this.MaxDamage = rand.Next(70, 100);
            }
            else if (tierChance < 0.5)
            {
                this.MinDamage = rand.Next(1, 50);
                this.MaxDamage = rand.Next(50, 75);
            }
            else
            {
                this.MinDamage = rand.Next(1, 30);
                this.MaxDamage = rand.Next(30, 50);
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsModified => Slayer != SlayerName.None;

        public override int AosStrengthReq => 80;
        public override int AosDexterityReq => 80;
        public override int OldStrengthReq => 80;
        public override int OldDexterityReq => 80;

        public override bool CanEquip(Mobile from)
        {
            if (IsModified)
            {
                return base.CanEquip(from);
            }

            from.SendMessage("You cannot equip this bow until a bowyer modifies it.");
            return false;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsModified)
            {
                from.SendMessage("That has already been modified.");
            }
            else if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("This must be in your backpack to modify it.");
            }
            else if (from.Skills[SkillName.Fletching].Base < 100.0)
            {
                from.SendMessage("Only a grandmaster bowcrafter can modify this weapon.");
            }
            else
            {
                from.BeginTarget(2, false, Targeting.TargetFlags.None, new TargetCallback(OnTargetGears));
                from.SendMessage("Select the gears you wish to use.");
            }
        }

        public void OnTargetGears(Mobile from, object targ)
        {
            Gears g = targ as Gears;

            if (g == null || !g.IsChildOf(from.Backpack))
            {
                from.SendMessage("Those are not gears.");
            }
            else if (IsModified)
            {
                from.SendMessage("That has already been modified.");
            }
            else if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("This must be in your backpack to modify it.");
            }
            else if (from.Skills[SkillName.Fletching].Base < 100.0)
            {
                from.SendMessage("Only a grandmaster bowcrafter can modify this weapon.");
            }
            else
            {
                g.Consume();

                Hue = 0x453;
                Slayer = (SlayerName)Utility.Random(2, 25);

                from.SendMessage("You modify it.");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
