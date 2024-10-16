using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("an inferno python corpse")]
    public class InfernoPython : BaseCreature
    {
        private DateTime m_NextFlamingConstriction;
        private DateTime m_NextInfernoStrike;
        private DateTime m_NextVolcanicEruption;
        private DateTime m_NextSummonFireElemental;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public InfernoPython()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Inferno Python";
            Body = 0x15; // GiantSerpent body
            Hue = 1774; // Unique fiery hue
			BaseSoundID = 219;

            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public InfernoPython(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}	

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextFlamingConstriction = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextInfernoStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_NextVolcanicEruption = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSummonFireElemental = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFlamingConstriction)
                {
                    FlamingConstriction();
                }

                if (DateTime.UtcNow >= m_NextInfernoStrike)
                {
                    InfernoStrike();
                }

                if (DateTime.UtcNow >= m_NextVolcanicEruption)
                {
                    VolcanicEruption();
                }

                if (DateTime.UtcNow >= m_NextSummonFireElemental)
                {
                    SummonFireElemental();
                }
            }
        }

        private void FlamingConstriction()
        {
            if (Combatant != null && Combatant is Mobile mobileCombatant && mobileCombatant.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Inferno Python constricts fiercely! *");
                mobileCombatant.SendMessage("You are burned by the Inferno Python’s constriction!");

                // Apply fire damage over time
                Timer.DelayCall(TimeSpan.FromSeconds(1), () => ApplyFireDamage(mobileCombatant));

                m_NextFlamingConstriction = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Shorter cooldown
            }
        }

        private void ApplyFireDamage(Mobile target)
        {
            if (target != null && target.Alive)
            {
                int damage = Utility.RandomMinMax(15, 20);
                AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);
                target.SendMessage("You continue to burn from the Inferno Python’s constriction!");
            }
        }

        private void InfernoStrike()
        {
            if (Combatant != null && Combatant is Mobile mobileCombatant && mobileCombatant.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Inferno Python strikes with fiery venom! *");
                mobileCombatant.SendMessage("The Inferno Python’s bite scorches you!");

                // Apply fiery bite effect
                mobileCombatant.PlaySound(0x208);
                mobileCombatant.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);

                int damage = Utility.RandomMinMax(20, 30);
                AOS.Damage(mobileCombatant, this, damage, 0, 100, 0, 0, 0);

                m_NextInfernoStrike = DateTime.UtcNow + TimeSpan.FromSeconds(25); // Shorter cooldown
            }
        }

        private void VolcanicEruption()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The ground erupts in flames! *");
                Effects.PlaySound(Location, Map, 0x307);
                Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10);

                foreach (Mobile m in GetMobilesInRange(5)) // Increased range
                {
                    if (m != this && m.Alive && !m.IsDeadBondedPet)
                    {
                        int damage = Utility.RandomMinMax(25, 35);
                        AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                        m.SendMessage("The volcanic eruption burns you!");
                    }
                }

                m_NextVolcanicEruption = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Shorter cooldown
            }
        }

        private void SummonFireElemental()
        {
            if (Combatant != null && Combatant.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Inferno Python summons a fiery ally! *");

                Point3D spawnLocation = Location;
                Effects.SendLocationParticles(EffectItem.Create(spawnLocation, Map, EffectItem.DefaultDuration), 0x374A, 10, 30, 5005);

                FireElemental elemental = new FireElemental();
                elemental.MoveToWorld(spawnLocation, Map);
                elemental.Combatant = Combatant;

                m_NextSummonFireElemental = DateTime.UtcNow + TimeSpan.FromMinutes(1.5); // Cooldown for summoning
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
