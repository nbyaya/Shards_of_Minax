using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a runestone golem corpse")]
    public class RunestoneGolem : BaseCreature
    {
        [Constructable]
        public RunestoneGolem()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Runestone Golem";
            Body = 14;
            BaseSoundID = 268;

            Hue = 2101; // Unique granite-like hue

            SetStr(200, 250);
            SetDex(50, 70);
            SetInt(100, 120);

            SetHits(220, 260);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 25, 35);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 90.0, 110.0);
            SetSkill(SkillName.Wrestling, 85.0, 105.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 50;
            ControlSlots = 3;

            PackItem(new Granite());
            PackItem(new MandrakeRoot());

            if (Utility.RandomDouble() < 0.02)
                PackItem(new StoneheartPendant());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);
            // Special: 10% chance to petrify target briefly
            if (Utility.RandomDouble() < 0.10)
            {
                defender.SendMessage(0x22, "The runes flash and your limbs stiffen!");
                defender.Freeze(TimeSpan.FromSeconds(2.5));
                Effects.SendTargetParticles(defender, 0x376A, 10, 15, 2023, EffectLayer.CenterFeet);
                defender.PlaySound(0x208);
            }
        }

        public RunestoneGolem(Serial serial) : base(serial) { }

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
