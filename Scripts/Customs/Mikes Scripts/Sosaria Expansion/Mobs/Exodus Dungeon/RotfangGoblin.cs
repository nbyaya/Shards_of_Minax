using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a rotfang goblin corpse")]
    public class RotfangGoblin : BaseCreature
    {
        private DateTime m_NextSporeBurst;
        private DateTime m_NextToxicLunge;
        private DateTime m_NextEnrageShriek;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public RotfangGoblin()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Rotfang Goblin";
            Body = 723;
            BaseSoundID = 0x600;
            Hue = 1260; // Sickly green hue

            SetStr(400, 500);
            SetDex(120, 160);
            SetInt(150, 200);

            SetHits(500, 650);
            SetStam(120, 160);
            SetMana(150, 200);

            SetDamage(18, 28);
            SetDamageType(ResistanceType.Poison, 50);
            SetDamageType(ResistanceType.Physical, 50);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 70, 85);
            SetResistance(ResistanceType.Energy, 25, 35);

            SetSkill(SkillName.MagicResist, 100.0, 130.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);
            SetSkill(SkillName.Poisoning, 80.0, 100.0);

            Fame = 8000;
            Karma = -8000;

            VirtualArmor = 45;

            m_AbilitiesInitialized = false;
        }

        public RotfangGoblin(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    var rand = new Random();
                    m_NextSporeBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 15));
                    m_NextToxicLunge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(8, 20));
                    m_NextEnrageShriek = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(12, 25));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextSporeBurst)
                    SporeBurst();

                if (DateTime.UtcNow >= m_NextToxicLunge)
                    ToxicLunge();

                if (DateTime.UtcNow >= m_NextEnrageShriek)
                    EnrageShriek();
            }
        }

        private void SporeBurst()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, false, "*The Rotfang Goblin releases a burst of toxic spores!*");
            PlaySound(0x231); // Poison gas sound
            Effects.SendLocationEffect(Location, Map, 0x113A, 16);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && m is PlayerMobile)
                {
                    m.ApplyPoison(this, Poison.Greater);
                    m.SendMessage(0x44, "Toxic spores burn your lungs!");
                }
            }

            m_NextSporeBurst = DateTime.UtcNow + TimeSpan.FromSeconds(20 + Utility.Random(10));
        }

        private void ToxicLunge()
        {
            if (Combatant is Mobile target && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The goblin lunges with toxic fangs!*");
                PlaySound(0x5FD); // Attack sound

                AOS.Damage(target, this, Utility.RandomMinMax(15, 25), 0, 0, 0, 0, 100);
                target.ApplyPoison(this, Poison.Deadly);

                m_NextToxicLunge = DateTime.UtcNow + TimeSpan.FromSeconds(15 + Utility.Random(10));
            }
        }

        private void EnrageShriek()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, false, "*The Rotfang Goblin shrieks and enters a frenzy!*");
            PlaySound(0x5FE); // Death/shriek sound

            this.SetDamage(25, 40);
            this.VirtualArmor = 20;

            Timer.DelayCall(TimeSpan.FromSeconds(8), () =>
            {
                this.SetDamage(18, 28);
                this.VirtualArmor = 45;
            });

            m_NextEnrageShriek = DateTime.UtcNow + TimeSpan.FromSeconds(30 + Utility.Random(10));
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Meager, 2);

            if (Utility.RandomDouble() < 0.01)
                PackItem(new RotfangVenom());
        }

        public override bool CanRummageCorpses => true;
        public override int Meat => 1;
        public override int TreasureMapLevel => 3;

        public override int GetAngerSound() => 0x600;
        public override int GetIdleSound() => 0x600;
        public override int GetAttackSound() => 0x5FD;
        public override int GetHurtSound() => 0x5FF;
        public override int GetDeathSound() => 0x5FE;

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

    public class RotfangVenom : Item
    {
        [Constructable]
        public RotfangVenom() : base(0xF0E) // vial graphic
        {
            Name = "Rotfang Venom";
            Hue = 1260;
            Weight = 1.0;
        }

        public RotfangVenom(Serial serial) : base(serial) { }

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
