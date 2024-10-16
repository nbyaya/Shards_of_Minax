using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an eldritch hare corpse")]
    public class EldritchHare : BaseCreature
    {
        private DateTime m_NextTentacleLash;
        private DateTime m_NextGlimpseOfMadness;
        private DateTime m_NextEldritchHowl;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public EldritchHare()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Eldritch Hare";
            Body = 205; // Rabbit body
            Hue = 2256; // Unique hue for the Eldritch Hare

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

        public EldritchHare(Serial serial)
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
        public override int GetAttackSound() 
        { 
            return 0xC9; 
        }

        public override int GetHurtSound() 
        { 
            return 0xCA; 
        }

        public override int GetDeathSound() 
        { 
            return 0xCB; 
        }
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextTentacleLash = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextGlimpseOfMadness = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextEldritchHowl = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextTentacleLash)
                {
                    TentacleLash();
                }

                if (DateTime.UtcNow >= m_NextGlimpseOfMadness)
                {
                    GlimpseOfMadness();
                }

                if (DateTime.UtcNow >= m_NextEldritchHowl)
                {
                    EldritchHowl();
                }
            }
        }

		private void TentacleLash()
		{
			PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Eldritch Hare lashes out with writhing tentacles! *");
			PlaySound(0x208); // Tentacle lash sound

			if (Combatant != null)
			{
				int damage = Utility.RandomMinMax(10, 20);
				AOS.Damage(Combatant, this, damage, 0, 100, 0, 0, 0);

				// Cast Combatant to Mobile
				Mobile mobileCombatant = Combatant as Mobile;
				if (mobileCombatant != null)
				{
					mobileCombatant.SendMessage("You are struck by a flurry of tentacles!");
					mobileCombatant.Freeze(TimeSpan.FromSeconds(2)); // Stun for 2 seconds

					// Add a visual effect for Tentacle Lash
					Effects.SendTargetParticles(mobileCombatant, 0x2D1A, 10, 20, 0, 0, 0, EffectLayer.Head, 0x1F4);
				}
			}

			m_NextTentacleLash = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for TentacleLash
		}


        private void GlimpseOfMadness()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Eldritch Hare releases a maddening aura! *");
            PlaySound(0x1D2); // Madness sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.SendMessage("You are overwhelmed by horrifying illusions!");
                    m.FixedParticles(0x374A, 10, 15, 0x0, 0, 0, EffectLayer.Head);
                    
                    // Apply confusion debuff
                    m.SendMessage("Your mind reels from the madness!");
                    m.AddStatMod(new StatMod(StatType.Str, "Eldritch Madness", -10, TimeSpan.FromSeconds(10)));
                    m.AddStatMod(new StatMod(StatType.Dex, "Eldritch Madness", -10, TimeSpan.FromSeconds(10)));
                    m.AddStatMod(new StatMod(StatType.Int, "Eldritch Madness", -10, TimeSpan.FromSeconds(10)));
                }
            }

            m_NextGlimpseOfMadness = DateTime.UtcNow + TimeSpan.FromSeconds(25); // Cooldown for GlimpseOfMadness
        }

        private void EldritchHowl()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Eldritch Hare emits a terrifying howl! *");
            PlaySound(0x1D2); // Howl sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.SendMessage("You are struck by a wave of fear and flee in terror!");

                    // Instead of Flee(), apply a fear debuff or use other methods to simulate fleeing
                    m.AddStatMod(new StatMod(StatType.Dex, "Eldritch Fear", -10, TimeSpan.FromSeconds(10)));

                    // Add a visual effect for Eldritch Howl
                    Effects.SendLocationEffect(m.Location, m.Map, 0x3709, 30, 10);
                }
            }

            m_NextEldritchHowl = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for EldritchHowl
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
