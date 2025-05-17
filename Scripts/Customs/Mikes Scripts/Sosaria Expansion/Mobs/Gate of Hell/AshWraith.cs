using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an ashen husk")]
    public class AshWraith : BaseCreature
    {
        [Constructable]
        public AshWraith()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Ash Wraith";
            Body = 26;
            Hue = 0x497; // smoky-red-gray hue
            BaseSoundID = 0x482;

            SetStr(90, 110);
            SetDex(80, 100);
            SetInt(80, 100);

            SetHits(70, 85);
            SetDamage(10, 14);

            SetDamageType(ResistanceType.Fire, 70);
            SetDamageType(ResistanceType.Physical, 30);

            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 20, 30);

            SetSkill(SkillName.Magery, 65.0, 80.0);
            SetSkill(SkillName.EvalInt, 65.0, 80.0);
            SetSkill(SkillName.MagicResist, 60.0, 75.0);
            SetSkill(SkillName.Tactics, 60.0, 70.0);
            SetSkill(SkillName.Wrestling, 55.0, 65.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 32;

            PackItem(new SulfurousAsh(Utility.RandomMinMax(5, 10)));

            if (Utility.RandomDouble() < 0.02)
                PackItem(new EmbercoreFragment()); // Unique quest drop/item
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
        }

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);
            if (Utility.RandomDouble() < 0.2)
            {
                defender.SendMessage(0x22, "The Ash Wraith's flame scorches you!");
                defender.Damage(10, this); // Fire backlash
                defender.AddStatMod(new StatMod(StatType.Dex, "WraithFlame", -5, TimeSpan.FromSeconds(10)));
            }
        }

        public AshWraith(Serial serial) : base(serial) { }

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
