using System;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a living grimoire corpse")]
    public class Grimorie : BaseCreature
    {
        private DateTime m_NextRuneOfBinding;
        private DateTime m_NextArcaneBurst;
        private DateTime m_NextMysticWard;
        private DateTime m_NextSummonMinions;
        private DateTime m_NextTeleport;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public Grimorie()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Grimorie the Living Grimoire";
            Body = 4; // Gargoyle body
            Hue = 1674; // Mystical hue
			BaseSoundID = 372;

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

        public Grimorie(Serial serial)
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
                    m_NextRuneOfBinding = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextArcaneBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextMysticWard = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_NextTeleport = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextRuneOfBinding)
                {
                    RuneOfBinding();
                }

                if (DateTime.UtcNow >= m_NextArcaneBurst)
                {
                    ArcaneBurst();
                }

                if (DateTime.UtcNow >= m_NextMysticWard)
                {
                    MysticWard();
                }

                if (DateTime.UtcNow >= m_NextSummonMinions)
                {
                    SummonMinions();
                }

                if (DateTime.UtcNow >= m_NextTeleport)
                {
                    Teleport();
                }
            }
        }

        private void RuneOfBinding()
        {
            if (Combatant != null && Combatant.Alive)
            {
                Mobile target = Combatant as Mobile; // Cast Combatant to Mobile
                if (target != null)
                {
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Grimorie casts Rune of Binding *");
                    target.Freeze(TimeSpan.FromSeconds(4));
                    target.SendMessage("You are bound by an ancient rune!");
                    m_NextRuneOfBinding = DateTime.UtcNow + TimeSpan.FromMinutes(1.5); // Cooldown
                }
            }
        }

        private void ArcaneBurst()
        {
            if (Combatant != null && Combatant.Alive)
            {
                Mobile target = Combatant as Mobile; // Cast Combatant to Mobile
                if (target != null)
                {
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Grimorie unleashes an Arcane Burst *");
                    Effects.SendTargetEffect(target, 0x2D1F, 16);
                    int damage = Utility.RandomMinMax(25, 40);
                    target.Damage(damage, this);
                    target.SendMessage("You are struck by a powerful arcane burst!");
                    target.SendMessage("Your magical abilities are suppressed!");
                    m_NextArcaneBurst = DateTime.UtcNow + TimeSpan.FromMinutes(1.5); // Cooldown
                }
            }
        }

        private void MysticWard()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Grimorie activates Mystic Ward *");
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x376A, 10, 16, 5005);
            foreach (Mobile m in GetMobilesInRange(7))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("A protective ward reflects the spell back at you!");
                }
            }
            m_NextMysticWard = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown
        }

        private void SummonMinions()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Grimorie summons minions! *");
                for (int i = 0; i < 2; i++)
                {
                    Point3D loc = GetSpawnPosition(2);
                    if (loc != Point3D.Zero)
                    {
                        SummonedMinion minion = new SummonedMinion();
                        minion.MoveToWorld(loc, Map);
                    }
                }
                m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromMinutes(3); // Cooldown
            }
        }

        private void Teleport()
        {
            if (Combatant != null)
            {
                Point3D newLocation = GetSpawnPosition(10);
                if (newLocation != Point3D.Zero)
                {
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Grimorie teleports to a new location *");
                    Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3728, 10, 30, 2023);
                    MoveToWorld(newLocation, Map);
                    m_NextTeleport = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown
                }
            }
        }

        private Point3D GetSpawnPosition(int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(p))
                    return p;
            }

            return Point3D.Zero;
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

    public class SummonedMinion : BaseCreature
    {
        [Constructable]
        public SummonedMinion()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a summoned minion";
            Body = 4; // Gargoyle body
            Hue = 1155; // Mystical hue

            SetStr(100);
            SetDex(80);
            SetInt(50);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Energy, 100);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Fire, 15, 25);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.EvalInt, 50.1, 70.0);
            SetSkill(SkillName.Magery, 50.1, 70.0);
            SetSkill(SkillName.MagicResist, 50.1, 70.0);
            SetSkill(SkillName.Tactics, 30.1, 50.0);
            SetSkill(SkillName.Wrestling, 20.1, 40.0);

            Fame = 1000;
            Karma = -1000;

            VirtualArmor = 30;
        }

        public SummonedMinion(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null)
            {
                // Minion disappears if it has no combat target
                Delete();
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
        }
    }
}
