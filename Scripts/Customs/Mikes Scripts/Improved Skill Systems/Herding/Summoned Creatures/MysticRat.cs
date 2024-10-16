using System;

namespace Server.Mobiles
{
    [CorpseName("a mystic rat corpse")]
    public class MysticRat : BaseCreature
    {
        private DateTime m_NextMagicEffect;

        [Constructable]
        public MysticRat()
            : base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            this.Name = "a mystic rat";
            this.Body = 238;
            this.BaseSoundID = 0xCC;
            this.Hue = 1150; // Unique hue for the mystic rat

            this.SetStr(200);
            this.SetDex(110);
            this.SetInt(150);

            this.SetDamage(14, 21);

            this.SetDamageType(ResistanceType.Physical, 0);
            this.SetDamageType(ResistanceType.Poison, 100);

            this.SetResistance(ResistanceType.Physical, 45, 55);
            this.SetResistance(ResistanceType.Fire, 50, 60);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 70, 80);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            this.SetSkill(SkillName.EvalInt, 90.1, 100.0);
            this.SetSkill(SkillName.Meditation, 90.1, 100.0);
            this.SetSkill(SkillName.Magery, 90.1, 100.0);
            this.SetSkill(SkillName.MagicResist, 90.1, 100.0);
            this.SetSkill(SkillName.Tactics, 100.0);
            this.SetSkill(SkillName.Wrestling, 98.1, 99.0);

            this.VirtualArmor = 58;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -18.9;

            m_NextMagicEffect = DateTime.UtcNow;
        }

        public MysticRat(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 1; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish | FoodType.Eggs | FoodType.GrainsAndHay; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextMagicEffect)
            {
                InflictMagicEffect();
            }
        }

        private void InflictMagicEffect()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                int effect = Utility.RandomMinMax(1, 3);

                switch (effect)
                {
                    case 1:
                        // Inflict fire damage
                        target.Damage(Utility.RandomMinMax(5, 10), this);
                        target.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                        target.PlaySound(0x208);
                        break;
                    case 2:
                        // Inflict poison
                        target.ApplyPoison(this, Poison.Regular);
                        target.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
                        target.PlaySound(0x205);
                        break;
                    case 3:
                        // Inflict curse
                        target.Paralyze(TimeSpan.FromSeconds(Utility.RandomMinMax(2, 4)));
                        target.FixedParticles(0x376A, 10, 15, 5023, EffectLayer.Waist);
                        target.PlaySound(0x204);
                        break;
                }

                m_NextMagicEffect = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            }
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Rich);
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
