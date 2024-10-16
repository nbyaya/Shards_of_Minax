using System;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a colossal toad corpse")]
    [TypeAlias("Server.Mobiles.ColossalToad")]
    public class ColossalToad : BaseCreature
    {
        private DateTime m_NextTongueLash;

        [Constructable]
        public ColossalToad()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a colossal toad";
            this.Body = 80;
            this.BaseSoundID = 0x26B;

            this.SetStr(200, 250);
            this.SetDex(50, 70);
            this.SetInt(60, 80);

            this.SetHits(200, 300);
            this.SetMana(50, 70);

            this.SetDamage(15, 25);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 35, 45);
            this.SetResistance(ResistanceType.Fire, 20, 30);
            this.SetResistance(ResistanceType.Cold, 30, 40);
            this.SetResistance(ResistanceType.Poison, 40, 50);
            this.SetResistance(ResistanceType.Energy, 25, 35);

            this.SetSkill(SkillName.MagicResist, 75.1, 90.0);
            this.SetSkill(SkillName.Tactics, 80.1, 100.0);
            this.SetSkill(SkillName.Wrestling, 85.1, 100.0);

            this.Fame = 4500;
            this.Karma = -4500;

            this.VirtualArmor = 50;

            this.Tamable = true;
            this.ControlSlots = 1;
            this.MinTameSkill = -10;

            this.Hue = 1372; // Unique hue

            m_NextTongueLash = DateTime.UtcNow;
        }

        public ColossalToad(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextTongueLash)
            {
                TongueLash();
            }
        }

        private void TongueLash()
        {
            Mobile target = Combatant as Mobile;

            if (target != null && target.Alive && !target.Paralyzed)
            {
                this.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*lashes its tongue*");
                target.Paralyze(TimeSpan.FromSeconds(3.0));
                target.FixedParticles(0x376A, 10, 15, 5030, EffectLayer.Head);
                target.PlaySound(0x22F);
                m_NextTongueLash = DateTime.UtcNow + TimeSpan.FromSeconds(15.0);
            }
        }

        public override int Hides
        {
            get
            {
                return 12;
            }
        }
        public override HideType HideType
        {
            get
            {
                return HideType.Spined;
            }
        }
        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.Fish | FoodType.Meat;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Rich);
        }
		
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lethal;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write(m_NextTongueLash);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version >= 1)
            {
                m_NextTongueLash = reader.ReadDateTime();
            }
            else
            {
                m_NextTongueLash = DateTime.UtcNow;
            }
        }
    }
}
