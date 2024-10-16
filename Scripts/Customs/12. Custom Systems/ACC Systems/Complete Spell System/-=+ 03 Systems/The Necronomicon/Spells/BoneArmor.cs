using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.NecromancyMagic
{
    public class BoneArmorSpell : NecromancySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Bone Armor", "Skeletal Armament",
            21004, // Animation ID for Bone Armor effect
            9300   // Sound ID for casting Bone Armor
        );

        public override SpellCircle Circle { get { return SpellCircle.Third; } }        
		public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public BoneArmorSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Apply bone armor effect
                Caster.PlaySound(9300);
                Caster.FixedParticles(21004, 1, 30, 9943, EffectLayer.Waist);

                // Create Bone Armor Item
                BoneArmor armor = new BoneArmor();
                armor.Movable = false;
                armor.MoveToWorld(Caster.Location, Caster.Map);

                // Attach the armor to the caster
                Caster.AddItem(armor);

                // Set up a temporary buff for defense
                Caster.VirtualArmorMod += 30; // Increase defense by 30

                // Reflect a portion of physical damage
                Caster.MagicDamageAbsorb = 20; // Absorb 20% of incoming physical damage and reflect

                // Set a timer to remove the armor effect
                Timer.DelayCall(TimeSpan.FromSeconds(30), () => RemoveArmorEffect(Caster, armor));
            }

            FinishSequence();
        }

        private void RemoveArmorEffect(Mobile caster, BoneArmor armor)
        {
            if (armor != null && !armor.Deleted)
            {
                armor.Delete(); // Remove armor from the game world
            }

            caster.VirtualArmorMod -= 30; // Revert defense buff
            caster.MagicDamageAbsorb = 0; // Remove damage reflection
            caster.SendMessage("Your bone armor dissipates."); // Notify the player
        }
    }

    public class BoneArmor : Item
    {
        public BoneArmor() : base(0x13BF) // Item ID for Bone Armor graphic
        {
            Name = "Bone Armor";
            Hue = 1150; // Set hue for the armor (white bone color)
        }

        public BoneArmor(Serial serial) : base(serial)
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
