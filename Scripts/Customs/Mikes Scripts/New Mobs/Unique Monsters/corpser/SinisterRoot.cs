using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a sinister root corpse")]
    public class SinisterRoot : BaseCreature
    {
        private DateTime m_NextRootSnare;
        private DateTime m_NextCrushingGrip;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SinisterRoot()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = 8; // Corpser body
            Hue = 1384; // Dark green hue
			BaseSoundID = 684;

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
            SetResistance(ResistanceType.Poison, 100);
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

        public SinisterRoot(Serial serial)
            : base(serial)
        {
        }

		public override bool ReacquireOnMovement
		{
			get
			{
				return !Controlled;
			}
		}
		public override bool AutoDispel
		{
			get
			{
				return !Controlled;
			}
		}

		public override int TreasureMapLevel
		{
			get
			{
				return 5;
			}
		}
		
		public override bool CanAngerOnTame
		{
			get
			{
				return true;
			}
		}

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
                    m_NextRootSnare = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30));
                    m_NextCrushingGrip = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(15, 45));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextRootSnare)
                {
                    RootSnare(Combatant as Mobile);
                }

                if (DateTime.UtcNow >= m_NextCrushingGrip && InRange(Combatant, 2))
                {
                    CrushingGrip(Combatant as Mobile);
                }
            }
        }

        private void RootSnare(Mobile target)
        {
            if (target == null || !target.Alive || !InRange(target, 5))
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*Sinister Root ensnares you in its crushing grip!*");
            PlaySound(0x1F1); // Sound for Root Snare

            // Visual effect
            target.FixedParticles(0x375A, 10, 15, 0x96C, 0x0, 0, EffectLayer.Waist);
            target.PlaySound(0x1F1);

            // Apply snare effect
            target.Freeze(TimeSpan.FromSeconds(5)); // Freezing target
            Timer.DelayCall(TimeSpan.FromSeconds(5), () => 
            {
                if (target != null && !target.Deleted)
                    target.Freeze(TimeSpan.Zero); // Unfreeze after 5 seconds
            });

            // Damage over time
            Timer.DelayCall(TimeSpan.FromSeconds(1), () => ApplyDamageOverTime(target));
            m_NextRootSnare = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset cooldown after use
        }

        private void ApplyDamageOverTime(Mobile target)
        {
            if (target != null && !target.Deleted && target.Alive)
            {
                int damage = Utility.RandomMinMax(5, 15);
                AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*You feel the root's grip tightening!*");
                Timer.DelayCall(TimeSpan.FromSeconds(3), () => ApplyDamageOverTime(target));
            }
        }

        private void CrushingGrip(Mobile target)
        {
            if (target == null || !target.Alive || !InRange(target, 2))
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*Sinister Root crushes you with its grip!*");
            PlaySound(0x1F1); // Sound for Crushing Grip

            int damage = Utility.RandomMinMax(20, 40);
            AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);

            m_NextCrushingGrip = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Reset cooldown after use
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_NextRootSnare);
            writer.Write(m_NextCrushingGrip);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextRootSnare = reader.ReadDateTime();
            m_NextCrushingGrip = reader.ReadDateTime();
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
