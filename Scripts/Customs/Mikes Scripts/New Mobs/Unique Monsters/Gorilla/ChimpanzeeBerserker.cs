using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a chimpanzee berserker corpse")]
    public class ChimpanzeeBerserker : BaseCreature
    {
        private DateTime m_NextRage;
        private DateTime m_NextFrenziedAttack;
        private DateTime m_NextBananaBomb;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ChimpanzeeBerserker()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a Chimpanzee Berserker";
            Body = 0x1D; // Gorilla body
            Hue = 1966; // Unique hue
			this.BaseSoundID = 0x9E;

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

        public ChimpanzeeBerserker(Serial serial)
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
                    m_NextRage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextFrenziedAttack = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextBananaBomb = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextRage)
                {
                    Rage();
                }

                if (DateTime.UtcNow >= m_NextFrenziedAttack)
                {
                    FrenziedAttack();
                }

                if (DateTime.UtcNow >= m_NextBananaBomb)
                {
                    BananaBomb();
                }
            }
        }

        private void Rage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Chimpanzee Berserker enters a state of uncontrollable fury! *");
            PlaySound(0x208); // Roar sound

            // Increase damage and decrease defense
            this.SetDamage(20, 35);
            this.VirtualArmor = 20;

            m_NextRage = DateTime.UtcNow + TimeSpan.FromSeconds(120); // Cooldown for Rage
        }

        private void FrenziedAttack()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Chimpanzee Berserker unleashes a frenzied attack! *");
            PlaySound(0x208); // Roar sound

            // Rapid attack effect
            for (int i = 0; i < 5; i++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(i * 200), () =>
                {
                    if (Combatant != null)
                    {
                        AOS.Damage(Combatant, this, Utility.RandomMinMax(7, 12), 0, 100, 0, 0, 0);
                        if (Combatant is Mobile mobile)
                        {
                            mobile.SendMessage("You are hit by a rapid series of attacks!");
                        }
                    }
                });
            }

            m_NextFrenziedAttack = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for FrenziedAttack
        }

        private void BananaBomb()
        {
            if (Combatant != null && Utility.RandomDouble() < 0.25) // 25% chance
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Chimpanzee Berserker throws a Banana Bomb! *");
                PlaySound(0x208); // Throw sound

                Point3D loc = Location;
                BananaBombItem bomb = new BananaBombItem();
                bomb.MoveToWorld(loc, Map);

                Timer.DelayCall(TimeSpan.FromSeconds(2), () => ExplodeBananaBomb(bomb));

                m_NextBananaBomb = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for BananaBomb
            }
        }

        private void ExplodeBananaBomb(BananaBombItem bomb)
        {
            if (bomb.Deleted)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Banana Bomb explodes in a mess of peels! *");
            PlaySound(0x307); // Explosion sound

            Effects.SendLocationEffect(bomb.Location, Map, 0x36BD, 20, 10); // Explosion effect

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);

                    if (m is Mobile mobile)
                    {
                        mobile.SendMessage("You are hit by the explosive banana bomb!");
                    }
                    m.PlaySound(0x1DD); // Explosion sound
                }
            }

            bomb.Delete();
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

    public class BananaBombItem : Item
    {
        public BananaBombItem() : base(0x171D)
        {
            Movable = false;
        }

        public BananaBombItem(Serial serial) : base(serial)
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
