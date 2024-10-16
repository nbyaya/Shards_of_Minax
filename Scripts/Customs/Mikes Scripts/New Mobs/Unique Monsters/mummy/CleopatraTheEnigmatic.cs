using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cleopatra the enigmatic corpse")]
    public class CleopatraTheEnigmatic : BaseCreature
    {
        private DateTime m_NextCharm;
        private DateTime m_NextMirage;
        private DateTime m_NextShadowStrike;
        private DateTime m_NextPhantomVeil;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public CleopatraTheEnigmatic()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Cleopatra the Enigmatic";
            Body = 154; // Mummy body
            Hue = 2166; // Unique hue
			BaseSoundID = 471;

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

        public CleopatraTheEnigmatic(Serial serial)
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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextCharm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextMirage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextPhantomVeil = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextCharm)
                {
                    SeductiveCharm();
                }

                if (DateTime.UtcNow >= m_NextMirage)
                {
                    IllusionaryMirage();
                }

                if (DateTime.UtcNow >= m_NextShadowStrike)
                {
                    ShadowStrike();
                }

                if (DateTime.UtcNow >= m_NextPhantomVeil)
                {
                    PhantomVeil();
                }
            }
        }

        private void SeductiveCharm()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Cleopatra’s charm makes you turn against your allies!*");
            PlaySound(0x1A1); // Enchantment sound

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive && m != Combatant)
                {
                    Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                    {
                        if (m != null && !m.Deleted && m.Alive)
                        {
                            m.Combatant = this;
                            m.SendMessage("You are charmed by Cleopatra!");
                        }
                    });
                }
            }

            m_NextCharm = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for SeductiveCharm
        }

        private void IllusionaryMirage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Cleopatra conjures illusionary mirages to confuse her foes!*");
            PlaySound(0x1A3); // Illusion sound

            for (int i = 0; i < 5; i++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(i * 500), () =>
                {
                    if (this.Map != null)
                    {
                        Mobile mirage = new MirageDummy();
                        mirage.MoveToWorld(Location, Map);
                        mirage.Combatant = this.Combatant;
                        mirage.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* An illusionary mirage appears!*");
                    }
                });
            }

            // Summon a spirit to assist
            Timer.DelayCall(TimeSpan.FromSeconds(2), () =>
            {
                if (this.Map != null)
                {
                    Mobile spirit = new SpiritOfCleopatra();
                    spirit.MoveToWorld(Location, Map);
                    spirit.Combatant = this.Combatant;
                    spirit.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Cleopatra summons a powerful spirit to aid her!*");
                }
            });

            m_NextMirage = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for IllusionaryMirage
        }

        private void ShadowStrike()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Cleopatra performs a Shadow Strike!*");
            PlaySound(0x1A4); // Shadow strike sound

            if (Combatant != null)
            {
                AOS.Damage(Combatant, this, Utility.RandomMinMax(25, 35), 0, 100, 0, 0, 0);
				Mobile mobile = Combatant as Mobile;
				if (mobile != null)
				{
					mobile.SendMessage("You are struck by a shadowy attack!");
					mobile.Freeze(TimeSpan.FromSeconds(3)); // Slow down the target
				}
            }

            m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for ShadowStrike
        }

        private void PhantomVeil()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Cleopatra envelops herself in a Phantom Veil!*");
            PlaySound(0x1A5); // Phantom veil sound

            FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
            this.VirtualArmor += 20; // Increase armor temporarily
            Timer.DelayCall(TimeSpan.FromSeconds(10), () => 
            {
                this.VirtualArmor -= 20;
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Phantom Veil fades!*");
            });

            m_NextPhantomVeil = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for PhantomVeil
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

    public class MirageDummy : BaseCreature
    {
        [Constructable]
        public MirageDummy()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an illusionary mirage";
            Body = 154; // Mummy body
            Hue = 1155; // Same hue as Cleopatra

            SetStr(100, 150);
            SetDex(50, 70);
            SetInt(30, 50);

            SetHits(50, 75);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 10, 15);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.MagicResist, 20.0, 40.0);
            SetSkill(SkillName.Tactics, 20.0, 40.0);
            SetSkill(SkillName.Wrestling, 20.0, 40.0);

            Fame = 1000;
            Karma = -1000;

            VirtualArmor = 20;
            Tamable = false;
            ControlSlots = 0;
        }

        public MirageDummy(Serial serial)
            : base(serial)
        {
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            // No loot
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

    public class SpiritOfCleopatra : BaseCreature
    {
        [Constructable]
        public SpiritOfCleopatra()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Spirit of Cleopatra";
            Body = 0x190; // Spirit body
            Hue = 1155; // Same hue as Cleopatra

            SetStr(150, 200);
            SetDex(80, 100);
            SetInt(120, 150);

            SetHits(100, 150);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 60.0, 80.0);
            SetSkill(SkillName.Tactics, 40.0, 60.0);
            SetSkill(SkillName.Wrestling, 40.0, 60.0);

            Fame = 3000;
            Karma = -3000;

            VirtualArmor = 40;
        }

        public SpiritOfCleopatra(Serial serial)
            : base(serial)
        {
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            // No loot
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
