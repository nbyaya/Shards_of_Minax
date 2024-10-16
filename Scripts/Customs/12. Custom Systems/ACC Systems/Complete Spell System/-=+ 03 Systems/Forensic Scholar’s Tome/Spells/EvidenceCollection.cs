using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ForensicsMagic
{
    public class EvidenceCollection : ForensicsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Evidence Collection", "Detectus Clue!",
            21004, // Icon
            9300,  // Action ID
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 0.0; } }
        public override int RequiredMana { get { return 20; } }

        public EvidenceCollection(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Mobile caster = Caster;

                // Visual and sound effects
                Effects.SendLocationParticles(EffectItem.Create(caster.Location, caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                Effects.PlaySound(caster.Location, caster.Map, 0x1E3);

                // Check for caster's backpack
                if (caster.Backpack != null)
                {
                    // Create a MurderClue and place it in the caster's backpack
                    MurderClue clue = new MurderClue();
                    caster.Backpack.DropItem(clue);

                    // Send message to the caster
                    caster.SendMessage("A MurderClue has been placed in your backpack.");
                }
                else
                {
                    caster.SendMessage("You need a backpack to collect evidence.");
                }
            }

            FinishSequence();
        }
    }

    public class MurderClue : Item
    {
        [Constructable]
        public MurderClue() : base(0x14F0) // The item ID for the clue; you can change this to the desired item ID
        {
            Name = "Murder Clue";
            Hue = 0x835; // Color the item for visual differentiation
            Weight = 1.0; // Adjust the weight as desired
        }

        public MurderClue(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
