using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a shattered golem")]
    public class RuneguardPowerGolem : BaseCreature
    {
        [Constructable]
        public RuneguardPowerGolem() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.3, 0.6)
        {
            Name = "Power Golem";
            Body = 752;
            Hue = 1150; // Arcane silver-blue
            BaseSoundID = 541;

            SetStr(400, 500);
            SetDex(100, 120);
            SetInt(150, 180);

            SetHits(700, 900);
            SetMana(200);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Energy, 40);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 30, 50);
            SetResistance(ResistanceType.Cold, 20, 35);
            SetResistance(ResistanceType.Poison, 90);
            SetResistance(ResistanceType.Energy, 50, 65);

            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 90.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 140.0);
            SetSkill(SkillName.Anatomy, 50.0, 70.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 60;

            PackItem(new IronIngot(Utility.RandomMinMax(10, 15)));

            // Drop the quest item
            if (Utility.RandomDouble() < 0.6) // 60% drop rate
                PackItem(new RunedStoneFragment());

            // Optional rare loot
            if (Utility.RandomDouble() < 0.05)
                PackItem(new PowerCrystal());

            if (Utility.RandomDouble() < 0.02)
                PackItem(new ClockworkAssembly());
        }

        public override bool AutoDispel => true;
        public override bool BardImmune => true;
        public override bool BleedImmune => true;
        public override bool Unprovokable => true;
        public override bool Uncalmable => true;

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            // Optional: Pulse Energy Backlash on heavy hit
            if (amount > 40 && Utility.RandomDouble() < 0.25)
            {
                from.SendMessage("The golem discharges a pulse of static energy!");
                from.Mana -= 10;
                from.Damage(Utility.RandomMinMax(10, 20), this);
                Effects.SendLocationEffect(from.Location, from.Map, 0x374A, 16, 1, Hue, 0);
                from.PlaySound(0x5AA);
            }

            base.OnDamage(amount, from, willKill);
        }

        public RuneguardPowerGolem(Serial serial) : base(serial) { }

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
