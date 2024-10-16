using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of a shadowblade assassin")]
    public class ShadowbladeAssassin : BaseCreature
    {
        private DateTime m_NextShadowStrike;
        private DateTime m_NextCloakOfShadows;
        private DateTime m_CloakOfShadowsEnd;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ShadowbladeAssassin()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a shadowblade assassin";
            Body = 57; // BoneKnight body
            BaseSoundID = 0x482;
            Hue = 2367; // Dark purple hue

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

        public ShadowbladeAssassin(Serial serial)
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

        public override bool AlwaysMurderer { get { return true; } }
        public override bool BardImmune { get { return !Core.AOS; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextCloakOfShadows = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextShadowStrike)
                {
                    DoShadowStrike();
                }

                if (DateTime.UtcNow >= m_NextCloakOfShadows && DateTime.UtcNow >= m_CloakOfShadowsEnd)
                {
                    DoCloakOfShadows();
                }
            }

            if (DateTime.UtcNow >= m_CloakOfShadowsEnd && m_CloakOfShadowsEnd != DateTime.MinValue)
            {
                DeactivateCloakOfShadows();
            }
        }

        private void DoShadowStrike()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive && target.InRange(this, 10))
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Shadow Strike *");
                PlaySound(0x22F);

                Point3D oldLocation = Location;
                Point3D newLocation = target.Location;

                // Create shadow afterimage
                Effects.SendLocationParticles(EffectItem.Create(oldLocation, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);

                // Teleport
                MoveToWorld(newLocation, Map);
                ProcessDelta();

                // Deal extra damage
                int damage = Utility.RandomMinMax(20, 30);
                AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);

                m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Reset cooldown
            }
        }

        private void DoCloakOfShadows()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Cloak of Shadows *");
            PlaySound(0x22F);
            FixedEffect(0x376A, 10, 15);

            Hidden = true;
            Blessed = true;

            m_CloakOfShadowsEnd = DateTime.UtcNow + TimeSpan.FromSeconds(10); // Duration of the effect
            m_NextCloakOfShadows = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown
        }

        private void DeactivateCloakOfShadows()
        {
            Hidden = false;
            Blessed = false;

            m_CloakOfShadowsEnd = DateTime.MinValue;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
