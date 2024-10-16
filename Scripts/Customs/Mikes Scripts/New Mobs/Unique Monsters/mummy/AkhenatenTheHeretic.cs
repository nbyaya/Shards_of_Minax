using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server.Regions;
using Server.Gumps;

namespace Server.Mobiles
{
    [CorpseName("an Akhenaten corpse")]
    public class AkhenatenTheHeretic : BaseCreature
    {
        private DateTime m_NextSunsWrath;
        private DateTime m_NextRevelation;
        private DateTime m_NextSolarEruption;
        private DateTime m_NextSummonMinions;
        private DateTime m_NextCurseOfThePharaoh;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public AkhenatenTheHeretic()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Akhenaten the Heretic";
            Body = 154; // Mummy body
            Hue = 2167; // Unique hue for Akhenaten
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

            m_AbilitiesInitialized = false;
        }

        public AkhenatenTheHeretic(Serial serial)
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
                    m_NextSunsWrath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextRevelation = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSolarEruption = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextCurseOfThePharaoh = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextSunsWrath)
                {
                    SunWrath();
                }

                if (DateTime.UtcNow >= m_NextRevelation)
                {
                    Revelation();
                }

                if (DateTime.UtcNow >= m_NextSolarEruption)
                {
                    SolarEruption();
                }

                if (DateTime.UtcNow >= m_NextSummonMinions)
                {
                    SummonMinions();
                }

                if (DateTime.UtcNow >= m_NextCurseOfThePharaoh)
                {
                    CurseOfThePharaoh();
                }
            }
        }

        private void SunWrath()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Akhenaten channels the sun’s fury! *");
            PlaySound(0x208); // Fiery sound

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 0, 100, 0, 0); // Fire damage

                    m.SendMessage("You are scorched by the intense heat of Akhenaten's wrath!");
                    m.MovingParticles(this, 0x36BD, 10, 0, false, true, 0x1F4, 0, 0x1B2, 0x1B2, 0xF1, 0); // Fiery beam effect
                }
            }

            m_NextSunsWrath = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for Sun’s Wrath
        }

        private void Revelation()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Akhenaten reveals the hidden! *");
            PlaySound(0x1F2); // Mystical sound

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.SendMessage("You are highlighted by a mystic light!");
                    Effects.SendLocationEffect(m.Location, m.Map, 0x376A, 10, 0, 0x3B2, 0); // Highlight effect
                }
            }

            m_NextRevelation = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for Revelation
        }

        private void SolarEruption()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Akhenaten unleashes a solar eruption! *");
            PlaySound(0x208); // Explosive sound

            Effects.SendLocationEffect(Location, Map, 0x36BD, 15, 0, 0x3B2, 0); // Fiery explosion effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(30, 40), 0, 0, 100, 0, 0); // Fire damage

                    m.SendMessage("You are engulfed in a fiery explosion!");
                }
            }

            m_NextSolarEruption = DateTime.UtcNow + TimeSpan.FromSeconds(80); // Cooldown for Solar Eruption
        }

        private void SummonMinions()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Akhenaten summons fiery minions to aid him! *");
            PlaySound(0x208); // Summon sound

            for (int i = 0; i < 3; i++)
            {
                FireImp imp = new FireImp();
                imp.Team = Team;
                imp.MoveToWorld(Location, Map);
                imp.Combatant = Combatant;
            }

            m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for Summon Minions
        }

        private void CurseOfThePharaoh()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Akhenaten casts the Curse of the Pharaoh! *");
            PlaySound(0x1F5); // Dark magic sound

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    m.SendMessage("You feel a dark curse weaken your resolve!");
                    m.SendMessage("Your damage is reduced and your movement is slowed!");

                    // Reduce damage and slow movement
                    m.Damage(0, this);
                    m.SendMessage("You have been cursed by Akhenaten!");
                    m.SendMessage("You feel your movements slow down!");
                }
            }

            m_NextCurseOfThePharaoh = DateTime.UtcNow + TimeSpan.FromSeconds(75); // Cooldown for Curse of the Pharaoh
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

    public class FireImp : BaseCreature
    {
        [Constructable]
        public FireImp()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a fiery imp";
            Body = 154; // Fire imp body
            Hue = 1155; // Fiery hue

            SetStr(100);
            SetDex(60);
            SetInt(40);

            SetHits(100);
            SetMana(50);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.MagicResist, 40.0, 60.0);
            SetSkill(SkillName.Tactics, 40.0, 60.0);
            SetSkill(SkillName.Wrestling, 40.0, 60.0);
            SetSkill(SkillName.EvalInt, 40.0, 60.0);

            Fame = 2000;
            Karma = -2000;

            VirtualArmor = 20;
        }

        public FireImp(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();
            if (Combatant != null)
            {
                // Simple behavior for the imp to attack the combatant
                if (Utility.RandomDouble() < 0.1) // 10% chance per tick
                {
                    CastFireball();
                }
            }
        }

        private void CastFireball()
        {
            if (Combatant != null)
            {
                // Send a fireball effect and deal damage
                Effects.SendLocationEffect(Combatant.Location, Combatant.Map, 0x36BD, 16, 0, 0x1F4, 0); // Fireball effect
                AOS.Damage(Combatant, this, Utility.RandomMinMax(10, 20), 0, 0, 100, 0, 0); // Fire damage
				Mobile mobile = Combatant as Mobile;
				if (mobile != null)
				{
					mobile.SendMessage("You are hit by a fireball from a fiery imp!");
				}
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
