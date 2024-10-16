using System;

namespace Server.Mobiles
{
    [CorpseName("a frost walrus corpse")]
    public class FrostWalrus : BaseCreature
    {
        [Constructable]
        public FrostWalrus()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            this.Name = "a frost walrus";
            this.Body = 0xDD;
            this.BaseSoundID = 0xE0;
            this.Hue = 1152; // Light blue hue

            this.SetStr(250);
            this.SetDex(80);
            this.SetInt(60);

            this.SetHits(200);
            this.SetMana(0);

            this.SetDamage(15, 25);

            this.SetDamageType(ResistanceType.Physical, 50);
            this.SetDamageType(ResistanceType.Cold, 50);

            this.SetResistance(ResistanceType.Physical, 50, 60);
            this.SetResistance(ResistanceType.Fire, 10, 20);
            this.SetResistance(ResistanceType.Cold, 70, 80);
            this.SetResistance(ResistanceType.Poison, 30, 40);
            this.SetResistance(ResistanceType.Energy, 30, 40);

            this.SetSkill(SkillName.MagicResist, 50.0, 75.0);
            this.SetSkill(SkillName.Tactics, 60.0, 80.0);
            this.SetSkill(SkillName.Wrestling, 60.0, 80.0);

            this.Fame = 3000;
            this.Karma = 3000;

            this.VirtualArmor = 50;

            this.Tamable = true;
            this.ControlSlots = 1;
            this.MinTameSkill = -10;
        }

        public FrostWalrus(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 2; } }
        public override int Hides { get { return 12; } }
        public override FoodType FavoriteFood { get { return FoodType.Fish; } }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (from != null && from != this && !willKill)
            {
                ReflectDamage(from);
            }
        }

        private void ReflectDamage(Mobile attacker)
        {
            int reflectAmount = Utility.RandomMinMax(5, 15); // Reflect 5-15 damage
            attacker.Damage(reflectAmount, this);
            attacker.SendLocalizedMessage(1070849); // You are struck by icy blubber!
            attacker.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
            attacker.PlaySound(0x10B);
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
