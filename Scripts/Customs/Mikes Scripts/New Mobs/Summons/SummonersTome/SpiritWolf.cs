using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a spirit wolf corpse")]
    public class SummonedSpiritWolf : BaseCreature
    {
        [Constructable]
        public SummonedSpiritWolf()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.3)
        {
            Name = "a spirit wolf";
            Body = 23;
            Hue = 0x47E; // Ethereal/glow effect
            BaseSoundID = 0xE5;

            SetStr(140, 160);
            SetDex(120, 140);
            SetInt(80, 100);

            SetHits(180, 220);
            SetMana(100);

            SetDamage(15, 22);
            SetDamageType(ResistanceType.Cold, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.MagicResist, 85.0, 100.0);
            SetSkill(SkillName.Tactics, 85.0, 100.0);
            SetSkill(SkillName.Wrestling, 85.0, 100.0);
            SetSkill(SkillName.SpiritSpeak, 100.0);
            SetSkill(SkillName.Meditation, 80.0);

            Fame = 5000;
            Karma = 0;

            VirtualArmor = 40;

            ControlSlots = 2;
            Tamable = false;
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            // 25% chance to chill the target, reducing their swing speed and movement
            if (Utility.RandomDouble() < 0.25)
            {
                defender.SendMessage("You feel a spectral chill slow your movements!");
                defender.Freeze(TimeSpan.FromSeconds(2.0));
                defender.PlaySound(0x5C9);
                defender.FixedParticles(0x376A, 10, 15, 5005, 1265, 2, EffectLayer.CenterFeet);
            }
        }

        public override bool BleedImmune => true;
        public override bool AutoDispel => true;
        public override Poison HitPoison => Poison.Regular;
        public override int GetAngerSound() => 0x165;

        public SummonedSpiritWolf(Serial serial) : base(serial) { }

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
