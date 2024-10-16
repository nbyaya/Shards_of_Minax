using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a stone guardian corpse")]
    public class StoneGuardian : BaseCreature
    {
        private DateTime m_NextRockShield;
        private DateTime m_NextGroundSlam;
        private bool m_RockShieldActive;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public StoneGuardian()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a stone guardian";
            Body = 14; // Earth Elemental body
            BaseSoundID = 268;
            Hue = 1499; // Unique stone-like hue

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

            PackItem(new IronOre(5));
            PackItem(new MandrakeRoot());

            // Initialize abilities
            m_AbilitiesInitialized = false; // Set the flag to false
        }

        public StoneGuardian(Serial serial)
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
        public override double WeaponAbilityChance { get { return 0.4; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextRockShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextGroundSlam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextRockShield)
                {
                    RockShield();
                }

                if (DateTime.UtcNow >= m_NextGroundSlam)
                {
                    GroundSlam();
                }
            }
        }

        private void RockShield()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* Summons a protective barrier of rocks *");
            PlaySound(0x15E);
            FixedEffect(0x376A, 10, 15);

            m_RockShieldActive = true;
            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(DeactivateRockShield));

            // Set next activation time with cooldown
            m_NextRockShield = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void DeactivateRockShield()
        {
            m_RockShieldActive = false;
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* The rock shield crumbles *");
        }

        private void GroundSlam()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* Slams the ground with tremendous force *");
            PlaySound(0x2F3);
            FixedEffect(0x3789, 10, 20);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = GetMobilesInRange(4);

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
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 100, 0, 0, 0, 0);
                m.SendLocalizedMessage(1072215); // The ground beneath you shakes and trembles.

                Point3D knockBackPoint = GetKnockBackPoint(m, 2);
                m.MovingEffect(m, 0x11B6, 5, 0, false, false, 0, 0); // Use 'm' as the first argument here
                m.MoveToWorld(knockBackPoint, Map);
            }

            // Set next activation time with cooldown
            m_NextGroundSlam = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private Point3D GetKnockBackPoint(Mobile m, int distance)
        {
            Point3D p = m.Location;

            for (int i = 0; i < distance; i++)
            {
                int x = p.X, y = p.Y;
                Movement.Movement.Offset(GetDirectionTo(m.Location), ref x, ref y);
                p = new Point3D(x, y, Map.GetAverageZ(x, y));
            }

            return p;
        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (m_RockShieldActive)
            {
                damage = (int)(damage * 0.5);
            }

            base.AlterMeleeDamageFrom(from, ref damage);
        }

        public override void AlterSpellDamageFrom(Mobile from, ref int damage)
        {
            if (m_RockShieldActive)
            {
                damage = (int)(damage * 0.5);
            }

            base.AlterSpellDamageFrom(from, ref damage);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_RockShieldActive);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_RockShieldActive = reader.ReadBool();

            // Reset initialization flag to reapply random intervals on load
            m_AbilitiesInitialized = false;
        }
    }
}
