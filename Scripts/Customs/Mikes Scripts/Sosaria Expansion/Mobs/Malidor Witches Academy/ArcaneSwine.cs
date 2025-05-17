using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an arcane swine corpse")]
    public class ArcaneSwine : BaseCreature
    {
        [Constructable]
        public ArcaneSwine() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.3, 0.5)
        {
            Name = "an Arcane Swine";
            Body = 0xCB;
            Hue = 1153;
            BaseSoundID = 0xC4;

            SetStr(40);
            SetDex(25);
            SetInt(70);

            SetHits(50);
            SetMana(100);

            SetDamage(4, 8);

            SetDamageType(ResistanceType.Energy, 100);

            SetResistance(ResistanceType.Physical, 20, 25);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.EvalInt, 60.0);
            SetSkill(SkillName.Magery, 60.0);
            SetSkill(SkillName.MagicResist, 40.0);
            SetSkill(SkillName.Tactics, 35.0);
            SetSkill(SkillName.Wrestling, 30.0);

            Fame = 2000;
            Karma = -1000;

            VirtualArmor = 24;
        }

        public override void GenerateLoot()
        {
            if (Utility.RandomDouble() < 0.75) // 75% chance
                PackItem(new LivingArcaneEssence());
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);
            if (Utility.RandomDouble() < 0.2)
            {
                attacker.SendMessage("Your limbs tingle as if struck by static!");
                attacker.Paralyze(TimeSpan.FromSeconds(1.5));
            }
        }

        public ArcaneSwine(Serial serial) : base(serial) { }

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
