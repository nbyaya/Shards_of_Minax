using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class ChivalryAugmentCrystal : Item
    {
        [Constructable]
        public ChivalryAugmentCrystal() : base(0x1F1C)  // Random item graphic, you can change this.
        {
            Name = "Chivalry Augment Crystal";
            Hue = 1109; // Random hue for differentiation, can be changed to your liking.
        }

        public ChivalryAugmentCrystal(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version.
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("The crystal needs to be in your pack.");
                return;
            }

            from.SendMessage("Which item would you like to augment for chivalry?");
            from.Target = new ChivalryAugmentTarget(this);
        }

        private class ChivalryAugmentTarget : Target
        {
            private ChivalryAugmentCrystal m_Crystal;

            public ChivalryAugmentTarget(ChivalryAugmentCrystal crystal) : base(1, false, TargetFlags.None)
            {
                m_Crystal = crystal;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Crystal.Deleted || m_Crystal.RootParent != from)
                    return;

                const SkillName chivalrySkill = SkillName.Chivalry;
                const int maxSlots = 5;

                int existingSlot = -1;
                int emptySlot = -1;

                // Loop through skill slots
                for (int i = 0; i < maxSlots; i++)
                {
                    SkillName skill;
                    double value;

                    if (targeted is BaseArmor armor)
                        armor.SkillBonuses.GetValues(i, out skill, out value);
                    else if (targeted is BaseClothing clothing)
                        clothing.SkillBonuses.GetValues(i, out skill, out value);
                    else if (targeted is BaseJewel jewel)
                        jewel.SkillBonuses.GetValues(i, out skill, out value);
                    else if (targeted is BaseWeapon weapon)
                        weapon.SkillBonuses.GetValues(i, out skill, out value);
                    else
                        continue;

                    if (skill == chivalrySkill)
                    {
                        existingSlot = i;
                        break;
                    }

                    if (value == 0 && emptySlot == -1) // Check for unused slot
                    {
                        emptySlot = i;
                    }
                }

                if (existingSlot != -1)
                {
                    // Increment existing bonus
                    SkillName skill;
                    double value;

                    if (targeted is BaseArmor armor)
                        armor.SkillBonuses.GetValues(existingSlot, out skill, out value);
                    else if (targeted is BaseClothing clothing)
                        clothing.SkillBonuses.GetValues(existingSlot, out skill, out value);
                    else if (targeted is BaseJewel jewel)
                        jewel.SkillBonuses.GetValues(existingSlot, out skill, out value);
                    else if (targeted is BaseWeapon weapon)
                        weapon.SkillBonuses.GetValues(existingSlot, out skill, out value);
                    else
                        return;

                    if (value >= 25)
                    {
                        from.SendMessage("The item already has the maximum chivalry bonus.");
                        return;
                    }

                    double newValue = value + 1;

                    if (targeted is BaseArmor armorToUpdate)
                        armorToUpdate.SkillBonuses.SetValues(existingSlot, chivalrySkill, newValue);
                    else if (targeted is BaseClothing clothingToUpdate)
                        clothingToUpdate.SkillBonuses.SetValues(existingSlot, chivalrySkill, newValue);
                    else if (targeted is BaseJewel jewelToUpdate)
                        jewelToUpdate.SkillBonuses.SetValues(existingSlot, chivalrySkill, newValue);
                    else if (targeted is BaseWeapon weaponToUpdate)
                        weaponToUpdate.SkillBonuses.SetValues(existingSlot, chivalrySkill, newValue);

                    from.SendMessage($"The chivalry bonus has been increased to +{newValue}.");
                    m_Crystal.Delete();
                }
                else if (emptySlot != -1)
                {
                    // Add new bonus
                    int bonus = Utility.Random(1, 25);

                    if (targeted is BaseArmor armorToUpdate)
                        armorToUpdate.SkillBonuses.SetValues(emptySlot, chivalrySkill, bonus);
                    else if (targeted is BaseClothing clothingToUpdate)
                        clothingToUpdate.SkillBonuses.SetValues(emptySlot, chivalrySkill, bonus);
                    else if (targeted is BaseJewel jewelToUpdate)
                        jewelToUpdate.SkillBonuses.SetValues(emptySlot, chivalrySkill, bonus);
                    else if (targeted is BaseWeapon weaponToUpdate)
                        weaponToUpdate.SkillBonuses.SetValues(emptySlot, chivalrySkill, bonus);

                    from.SendMessage($"The item has been augmented with a +{bonus} chivalry bonus.");
                    m_Crystal.Delete();
                }
                else
                {
                    from.SendMessage("All skill slots are occupied, and none can be used for chivalry.");
                }
            }
        }
    }
}
