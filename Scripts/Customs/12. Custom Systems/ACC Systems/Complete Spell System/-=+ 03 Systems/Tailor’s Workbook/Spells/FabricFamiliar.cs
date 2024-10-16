using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.TailoringMagic
{
    public class FabricFamiliarSpell : TailoringSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Fabric Familiar", "Fab Foh",
            21015,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; } // Example: Adjust the circle as needed
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public FabricFamiliarSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                try
                {
                    // Summon the Fabric Familiar
                    FabricFamiliar familiar = new FabricFamiliar(Caster);
                    SpellHelper.Summon(familiar, Caster, 0x217, TimeSpan.FromMinutes(2.0), false, false);

                    // Play a summoning effect and sound
                    Effects.SendLocationParticles(
                        EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 
                        0x376A, 9, 32, 5008
                    );
                    Caster.PlaySound(0x202);
                }
                catch (Exception ex)
                {
                    Caster.SendMessage("An error occurred while casting the spell.");
                    Console.WriteLine("Exception in FabricFamiliar.OnCast(): " + ex.Message);
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }

    public class FabricFamiliar : BaseCreature
    {
        private Mobile m_Caster; // To hold reference to the caster

        public FabricFamiliar(Mobile caster) : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            m_Caster = caster; // Set the caster reference

            Name = "Fabric Familiar";
            Body = 0x7D; // Fabric-like creature appearance
            BaseSoundID = 0x482; // Light creature sound

            SetStr(30);
            SetDex(40);
            SetInt(35);

            SetHits(50);
            SetMana(0);

            SetDamage(3, 5);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 15);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.MagicResist, 20.0);
            SetSkill(SkillName.Tactics, 20.0);
            SetSkill(SkillName.Wrestling, 20.0);

            Fame = 100;
            Karma = 100;

            VirtualArmor = 10;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Utility.RandomDouble() < 0.05) // 5% chance each tick to scout or gather
            {
                if (Utility.RandomBool())
                {
                    GatherMaterials();
                }
                else
                {
                    ScoutArea();
                }
            }
        }

        private void GatherMaterials()
        {
            // Example: Fabric Familiar gathers wool or wood from the environment
            if (m_Caster != null)
            {
                m_Caster.SendMessage("Your Fabric Familiar gathers some materials for you.");
                // Add logic to give materials to the caster or drop them on the ground
            }
        }

        private void ScoutArea()
        {
            // Example: Fabric Familiar scouts the nearby area
            if (m_Caster != null)
            {
                m_Caster.SendMessage("Your Fabric Familiar scouts the area and returns with information.");
                // Add logic to provide scouting information (e.g., nearby enemies or resources)
            }
        }

        public FabricFamiliar(Serial serial) : base(serial)
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
