using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a thunderous grizzly corpse")]
    public class ThunderousGrizzly : BaseCreature
    {
        private DateTime m_NextFuriousRoar;

        [Constructable]
        public ThunderousGrizzly()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a thunderous grizzly";
            Body = 212;
            BaseSoundID = 0xA3;
            Hue = 1175; // Dark thunderous hue

            this.SetStr(200, 250);
            this.SetDex(100, 120);
            this.SetInt(60, 80);

            this.SetHits(150, 200);
            this.SetMana(0);

            this.SetDamage(15, 25);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 40, 50);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 10, 20);
            this.SetResistance(ResistanceType.Energy, 10, 20);

            this.SetSkill(SkillName.MagicResist, 60.1, 80.0);
            this.SetSkill(SkillName.Tactics, 80.1, 100.0);
            this.SetSkill(SkillName.Wrestling, 70.1, 90.0);

            this.Fame = 4500;
            this.Karma = -4500;

            this.VirtualArmor = 50;

            this.Tamable = true;
            this.ControlSlots = 1;
            this.MinTameSkill = -10;
        }

        public ThunderousGrizzly(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 2; } }
        public override int Hides { get { return 20; } }
        public override FoodType FavoriteFood { get { return FoodType.Fish | FoodType.FruitsAndVegies | FoodType.Meat; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Bear; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextFuriousRoar)
            {
                FuriousRoar();
                m_NextFuriousRoar = DateTime.UtcNow + TimeSpan.FromSeconds(30); // 30 seconds cooldown
            }
        }

        private void FuriousRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*roars furiously*");
            PlaySound(0x21D);
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile enemy in GetMobilesInRange(5))
            {
                if (enemy != this && enemy.Combatant == this)
                {
                    enemy.SendMessage("You are terrified by the roar and feel weaker!");
                    enemy.PlaySound(0x1F2);
                    enemy.AddStatMod(new StatMod(StatType.Str, "FuriousRoarStr", -10, TimeSpan.FromSeconds(10)));
                    enemy.AddStatMod(new StatMod(StatType.Dex, "FuriousRoarDex", -10, TimeSpan.FromSeconds(10)));
                }
            }
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
        }
    }
}
