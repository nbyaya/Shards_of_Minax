using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a lava fiend corpse")]
    public class LavaFiend : BaseCreature
    {
        private DateTime m_NextLavaFlow;
        private DateTime m_NextVolcanicEruption;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public LavaFiend()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a lava fiend";
            Body = 14; // Earth Elemental body
            BaseSoundID = 268;
            Hue = 1500; // Fiery red-orange hue

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

            PackItem(new SulfurousAsh(3));
            PackItem(new IronOre(3));

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public LavaFiend(Serial serial)
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

        public override bool BleedImmune { get { return true; } }
        public override double WeaponAbilityChance { get { return 0.3; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextLavaFlow = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 20));
                    m_NextVolcanicEruption = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextLavaFlow)
                {
                    LavaFlow();
                }

                if (DateTime.UtcNow >= m_NextVolcanicEruption)
                {
                    VolcanicEruption();
                }
            }
        }

        private void LavaFlow()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* Unleashes a flow of molten lava *");
            PlaySound(0x241);
            FixedEffect(0x36B0, 10, 30);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = GetMobilesInRange(3);

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m))
                {
                    targets.Add(m);
                }
            }

            eable.Free();

            foreach (Mobile m in targets)
            {
                DoLavaFlowDamage(m);
            }

            Timer.DelayCall(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2), 4, new TimerStateCallback(LavaFlowDamage), targets);

            m_NextLavaFlow = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset cooldown after use
        }

        private void LavaFlowDamage(object state)
        {
            List<Mobile> targets = (List<Mobile>)state;

            foreach (Mobile m in targets)
            {
                if (m.Alive && m.Map != null && m.Map == this.Map && m.InRange(this, 3))
                {
                    DoLavaFlowDamage(m);
                }
            }
        }

        private void DoLavaFlowDamage(Mobile m)
        {
            int damage = Utility.RandomMinMax(10, 20);
            AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
            m.PlaySound(0x1DD);
        }

        private void VolcanicEruption()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* Erupts in a burst of molten lava *");
            PlaySound(0x5CF);
            FixedEffect(0x36B0, 10, 30);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = GetMobilesInRange(3);

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m))
                {
                    targets.Add(m);
                }
            }

            eable.Free();

            foreach (Mobile m in targets)
            {
                int damage = Utility.RandomMinMax(30, 40);
                AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                m.PlaySound(0x1DD);
            }

            m_NextVolcanicEruption = DateTime.UtcNow + TimeSpan.FromSeconds(25); // Reset cooldown after use
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

            // Reset ability initialization state when deserializing
            m_AbilitiesInitialized = false;
        }
    }
}
