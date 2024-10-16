using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a colossal turkey corpse")]
    public class ColossalTurkey : BaseCreature
    {
        private DateTime m_NextFeatherShield;

        [Constructable]
        public ColossalTurkey()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a colossal turkey";
            Body = 1026;
            BaseSoundID = 0x66A;
            Hue = 1153; // Unique hue for the special ability

            SetStr(200, 300);
            SetDex(170, 260);
            SetInt(430, 560);

            SetHits(25000);
            SetMana(1000);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 55, 70);
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 35, 45);
            SetResistance(ResistanceType.Poison, 45, 55);
            SetResistance(ResistanceType.Energy, 45, 55);

            SetSkill(SkillName.MagicResist, 85.0, 100.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);
            SetSkill(SkillName.Anatomy, 75.0, 80.0);

            VirtualArmor = 60;

            m_NextFeatherShield = DateTime.UtcNow;
        }

        public ColossalTurkey(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 1; } }
        public override MeatType MeatType { get { return MeatType.Bird; } }
        public override FoodType FavoriteFood { get { return FoodType.GrainsAndHay; } }
        public override int Feathers { get { return 25; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextFeatherShield)
            {
                ActivateFeatherShield();
            }
        }

        private void ActivateFeatherShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Feather Shield *");
            PlaySound(0x1FE);
            FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);

            VirtualArmor += 50;
            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(DeactivateFeatherShield));

            m_NextFeatherShield = DateTime.UtcNow + TimeSpan.FromMinutes(3);
        }

        private void DeactivateFeatherShield()
        {
            VirtualArmor -= 50;
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Feather Shield fades *");
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
        }

        public override int GetIdleSound()
        {
            return 0x66A;
        }

        public override int GetAngerSound()
        {
            return 0x66A;
        }

        public override int GetHurtSound()
        {
            return 0x66B;
        }

        public override int GetDeathSound()
        {
            return 0x66B;
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
            m_NextFeatherShield = DateTime.UtcNow;
        }
    }
}
