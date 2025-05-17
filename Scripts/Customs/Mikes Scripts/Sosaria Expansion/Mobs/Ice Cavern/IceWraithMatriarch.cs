using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an ice wraith matriarch corpse")]
    public class IceWraithMatriarch : BaseCreature
    {
        [Constructable]
        public IceWraithMatriarch()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Ice Wraith Matriarch";
            Body = 267;
            Hue = 1150; // Frosty blue
            BaseSoundID = 0x482;

            SetStr(180, 220);
            SetDex(110, 140);
            SetInt(90, 120);

            SetHits(380, 420);
            SetDamage(14, 18);

            SetDamageType(ResistanceType.Cold, 70);
            SetDamageType(ResistanceType.Physical, 30);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Cold, 70, 90);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 85.0, 100.0);
            SetSkill(SkillName.SpiritSpeak, 100.0);

            Fame = 8000;
            Karma = -8000;

            VirtualArmor = 34;

            PackItem(new BlackPearl(3));
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);
            if (Utility.RandomDouble() < 0.2)
            {
                attacker.Freeze(TimeSpan.FromSeconds(2));
                attacker.SendMessage("A chilling shriek paralyzes you!");
                Effects.SendLocationEffect(Location, Map, 0x376A, 16, 1, 1152, 0);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
        }

        public IceWraithMatriarch(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
