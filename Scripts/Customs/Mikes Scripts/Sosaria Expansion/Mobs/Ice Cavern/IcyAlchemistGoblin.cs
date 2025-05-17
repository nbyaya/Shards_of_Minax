using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an icy goblin corpse")]
    public class IcyAlchemistGoblin : BaseCreature
    {
        private DateTime m_NextIceBomb;
        private DateTime m_NextFrostNova;
        private DateTime m_NextGlacialArmor;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public IcyAlchemistGoblin()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an icy alchemist goblin";
            Body = 723;
            Hue = 1150; // Unique icy blue hue
            BaseSoundID = 0x600;

            SetStr(400, 500);
            SetDex(100, 150);
            SetInt(300, 350);

            SetHits(500, 650);
            SetStam(100, 150);
            SetMana(350, 450);

            SetDamage(12, 18);
            SetDamageType(ResistanceType.Cold, 80);
            SetDamageType(ResistanceType.Physical, 20);

            SetResistance(ResistanceType.Physical, 40, 55);
            SetResistance(ResistanceType.Fire, 15, 25);
            SetResistance(ResistanceType.Cold, 70, 90);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.EvalInt, 105.0, 115.0);
            SetSkill(SkillName.MagicResist, 100.0, 115.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 85.0, 95.0);
            SetSkill(SkillName.Alchemy, 100.0, 120.0);

            Fame = 12000;
            Karma = -12000;

            VirtualArmor = 60;

            m_AbilitiesInitialized = false;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && Combatant.Alive)
            {
                if (!m_AbilitiesInitialized)
                {
                    m_NextIceBomb = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(10, 20));
                    m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(15, 25));
                    m_NextGlacialArmor = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(30, 45));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextIceBomb)
                    ThrowIceBomb();

                if (DateTime.UtcNow >= m_NextFrostNova)
                    CastFrostNova();

                if (DateTime.UtcNow >= m_NextGlacialArmor)
                    ActivateGlacialArmor();
            }
        }

        private void ThrowIceBomb()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, false, "*hurls a volatile ice bomb!*");
            PlaySound(0x10B); // Ice crack sound

            if (Combatant != null)
            {
                Effects.SendLocationEffect(Combatant.Location, Combatant.Map, 0x376A, 20, 10);
                AOS.Damage(Combatant, this, Utility.RandomMinMax(20, 35), 0, 0, 0, 0, 100); // Full cold damage

                if (Combatant is Mobile target)
                {
                    target.SendMessage("You are struck by a freezing alchemical explosion!");
                    target.Freeze(TimeSpan.FromSeconds(1.5));
                }
            }

            m_NextIceBomb = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(15, 25));
        }

        private void CastFrostNova()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, false, "*unleashes a ring of icy fury!*");
            PlaySound(0x64E); // Ice nova sound
            Effects.SendLocationEffect(Location, Map, 0x3709, 20, 10);

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 0, 0, 0, 100);

                    if (m is Mobile mob)
                    {
                        mob.SendMessage("You are chilled to the bone by the Frost Nova!");
                        mob.Freeze(TimeSpan.FromSeconds(2));
                    }

                    m.PlaySound(0x145); // Cold hit sound
                }
            }

            m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(30, 45));
        }

        private void ActivateGlacialArmor()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*envelops itself in glacial armor*");
            PlaySound(0x5FE); // Defensive sound
            this.VirtualArmor += 30;

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                VirtualArmor -= 30;
                PublicOverheadMessage(MessageType.Regular, 0x480, true, "*the icy armor melts away*");
            });

            m_NextGlacialArmor = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(60, 90));
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 3);

            if (Utility.RandomDouble() < 0.01)
                PackItem(new AlchemistsExperimentalBracers());
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.03)
                c.DropItem(new IceboundFlask());
        }

        public override int GetAngerSound() => 0x600;
        public override int GetIdleSound() => 0x600;
        public override int GetAttackSound() => 0x5FD;
        public override int GetHurtSound() => 0x5FF;
        public override int GetDeathSound() => 0x5FE;

        public override bool CanRummageCorpses => true;
        public override int TreasureMapLevel => 3;
        public override int Meat => 1;

        public IcyAlchemistGoblin(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false;
        }
    }

    public class IceboundFlask : Item
    {
        public IceboundFlask() : base(0xF0E)
        {
            Name = "Icebound Flask";
            Hue = 1150;
            Weight = 1.0;
            LootType = LootType.Blessed;
        }

        public IceboundFlask(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
