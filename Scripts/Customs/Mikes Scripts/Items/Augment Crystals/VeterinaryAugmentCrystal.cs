using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class VeterinaryAugmentCrystal : Item
    {
        [Constructable]
        public VeterinaryAugmentCrystal() : base(0x1F1C)  // Random item graphic, you can change this.
        {
            Name = "Veterinary Augment Crystal";
            Hue = 1176; // Random hue for differentiation, can be changed to your liking.
        }

        public VeterinaryAugmentCrystal(Serial serial) : base(serial)
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

            from.SendMessage("Which item would you like to augment for veterinary?");
            from.Target = new VeterinaryAugmentTarget(this);
        }

        private class VeterinaryAugmentTarget : Target
        {
            private VeterinaryAugmentCrystal m_Crystal;

            public VeterinaryAugmentTarget(VeterinaryAugmentCrystal crystal) : base(1, false, TargetFlags.None)
            {
                m_Crystal = crystal;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Crystal.Deleted || m_Crystal.RootParent != from)
                    return;

                if (targeted is BaseArmor armor)
                {
                    AugmentItem(from, armor, m_Crystal);
                }
                else if (targeted is BaseClothing clothing)
                {
                    AugmentItem(from, clothing, m_Crystal);
                }
                else if (targeted is BaseJewel jewel)
                {
                    AugmentItem(from, jewel, m_Crystal);
                }
                else if (targeted is BaseWeapon weapon)
                {
                    AugmentItem(from, weapon, m_Crystal);
                }
                else
                {
                    from.SendMessage("You can't augment that item.");
                }
            }

            private void AugmentItem(Mobile from, Item item, VeterinaryAugmentCrystal crystal)
            {
                const int maxSlots = 5; // Number of skill slots
                const SkillName veterinarySkill = SkillName.Veterinary;

                int existingSlot = -1;
                int emptySlot = -1;

                // Loop through skill slots
                for (int i = 0; i < maxSlots; i++)
                {
                    SkillName skill;
                    double value;

                    if (item is BaseArmor armor)
                        armor.SkillBonuses.GetValues(i, out skill, out value);
                    else if (item is BaseClothing clothing)
                        clothing.SkillBonuses.GetValues(i, out skill, out value);
                    else if (item is BaseJewel jewel)
                        jewel.SkillBonuses.GetValues(i, out skill, out value);
                    else if (item is BaseWeapon weapon)
                        weapon.SkillBonuses.GetValues(i, out skill, out value);
                    else
                        continue;

                    if (skill == veterinarySkill)
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

                    if (item is BaseArmor armor)
                        armor.SkillBonuses.GetValues(existingSlot, out skill, out value);
                    else if (item is BaseClothing clothing)
                        clothing.SkillBonuses.GetValues(existingSlot, out skill, out value);
                    else if (item is BaseJewel jewel)
                        jewel.SkillBonuses.GetValues(existingSlot, out skill, out value);
                    else if (item is BaseWeapon weapon)
                        weapon.SkillBonuses.GetValues(existingSlot, out skill, out value);
                    else
                        return;

                    if (value >= 25)
                    {
                        from.SendMessage("The item already has the maximum veterinary bonus.");
                        return;
                    }

                    double newValue = value + 1;

                    if (item is BaseArmor armorToUpdate)
                        armorToUpdate.SkillBonuses.SetValues(existingSlot, veterinarySkill, newValue);
                    else if (item is BaseClothing clothingToUpdate)
                        clothingToUpdate.SkillBonuses.SetValues(existingSlot, veterinarySkill, newValue);
                    else if (item is BaseJewel jewelToUpdate)
                        jewelToUpdate.SkillBonuses.SetValues(existingSlot, veterinarySkill, newValue);
                    else if (item is BaseWeapon weaponToUpdate)
                        weaponToUpdate.SkillBonuses.SetValues(existingSlot, veterinarySkill, newValue);

                    from.SendMessage($"The veterinary bonus has been increased to +{newValue}.");
                    crystal.Delete();
                }
                else if (emptySlot != -1)
                {
                    // Add new bonus
                    int bonus = Utility.Random(1, 25);

                    if (item is BaseArmor armorToUpdate)
                        armorToUpdate.SkillBonuses.SetValues(emptySlot, veterinarySkill, bonus);
                    else if (item is BaseClothing clothingToUpdate)
                        clothingToUpdate.SkillBonuses.SetValues(emptySlot, veterinarySkill, bonus);
                    else if (item is BaseJewel jewelToUpdate)
                        jewelToUpdate.SkillBonuses.SetValues(emptySlot, veterinarySkill, bonus);
                    else if (item is BaseWeapon weaponToUpdate)
                        weaponToUpdate.SkillBonuses.SetValues(emptySlot, veterinarySkill, bonus);

                    from.SendMessage($"The item has been augmented with a +{bonus} veterinary bonus.");
                    crystal.Delete();
                }
                else
                {
                    from.SendMessage("All skill slots are occupied, and none can be used for veterinary.");
                }
            }
        }
    }
}
