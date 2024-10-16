using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a silkweb spider corpse")]
    public class SilkwebSpider : BaseCreature
    {
        private DateTime m_NextWebTrap;

        [Constructable]
        public SilkwebSpider()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a silkweb spider";
            Body = 28; // Same as the Giant Spider
            BaseSoundID = 0x388;
            Hue = 1150; // Mystical dark purple hue

            SetStr(150);
            SetDex(100);
            SetInt(50);

            SetHits(150);
            SetMana(0);

            SetDamage(12, 20);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Poison, 60, 70);

            SetSkill(SkillName.Poisoning, 80.0);
            SetSkill(SkillName.MagicResist, 70.0);
            SetSkill(SkillName.Tactics, 90.0);
            SetSkill(SkillName.Wrestling, 80.0);

            Fame = 2500;
            Karma = -2500;

            VirtualArmor = 50;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -10;

            PackItem(new SpidersSilk(10));
            m_NextWebTrap = DateTime.UtcNow;
        }

        public SilkwebSpider(Serial serial)
            : base(serial)
        {
        }

        public override FoodType FavoriteFood
        {
            get { return FoodType.Meat; }
        }

        public override PackInstinct PackInstinct
        {
            get { return PackInstinct.Arachnid; }
        }

        public override Poison PoisonImmune
        {
            get { return Poison.Greater; }
        }

        public override Poison HitPoison
        {
            get { return Poison.Deadly; }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextWebTrap)
            {
                CreateWebTrap();
                m_NextWebTrap = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Web trap cooldown
            }
        }

        private void CreateWebTrap()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive && target.Map == this.Map)
            {
                target.SendMessage("You are caught in a sticky web!");
                target.Paralyze(TimeSpan.FromSeconds(5));
                target.PlaySound(0x205);
                target.FixedEffect(0x376A, 10, 16);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((DateTime)m_NextWebTrap);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextWebTrap = reader.ReadDateTime();
        }
    }
}
