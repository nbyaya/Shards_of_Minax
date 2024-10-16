using System;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.CookingMagic
{
    public class PotionPie : CookingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Potion Pie", "Confectio Potionis",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 20; } }

        public PotionPie(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Mobile target = Caster; // Targets the caster for the pie summon
                Point3D loc = new Point3D(target.X + 1, target.Y, target.Z);

                // Summon the pie
                PotionPieItem pie = new PotionPieItem();
                pie.MoveToWorld(loc, Caster.Map);

                // Play a sound and visual effect
                Effects.PlaySound(loc, Caster.Map, 0x1E9); // Pie summoning sound
                Effects.SendLocationEffect(loc, Caster.Map, 0x3728, 10, 1, 1153, 4); // Visual effect for pie appearance

                Caster.SendMessage("You have summoned a Potion Pie!");
            }

            FinishSequence();
        }
    }

    public class PotionPieItem : Item
    {
        public override string DefaultName
        {
            get { return "Potion Pie"; }
        }

        [Constructable]
        public PotionPieItem() : base(0x1041) // Item ID for pie
        {
            Weight = 1.0;
            Hue = Utility.RandomList(1153, 1165, 1175, 1194); // Random color for visual variety
        }

        public PotionPieItem(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!Movable)
                return;

            if (!from.InRange(this.GetWorldLocation(), 1))
            {
                from.SendLocalizedMessage(500446); // That is too far away.
                return;
            }

            ApplyRandomPotionEffect(from);
            Delete();
        }

        private void ApplyRandomPotionEffect(Mobile from)
        {
            int randomEffect = Utility.Random(6);

            switch (randomEffect)
            {
                case 0:
                    from.SendMessage("The pie grants you a greater heal effect!");
                    from.Heal(Utility.RandomMinMax(20, 40));
                    Effects.SendTargetParticles(from, 0x376A, 9, 32, 5005, EffectLayer.Waist);
                    break;
                case 1:
                    from.SendMessage("The pie grants you a greater cure effect!");
                    from.CurePoison(from);
                    Effects.SendTargetParticles(from, 0x373A, 9, 32, 5005, EffectLayer.Waist);
                    break;
                case 2:
                    from.SendMessage("The pie grants you a refresh effect!");
                    from.Stam += Utility.RandomMinMax(10, 30);
                    Effects.SendTargetParticles(from, 0x3735, 1, 15, 9502, EffectLayer.Waist);
                    break;
                case 3:
                    from.SendMessage("The pie grants you a greater agility effect!");
                    BuffInfo.AddBuff(from, new BuffInfo(BuffIcon.Agility, 1044097, 1153, TimeSpan.FromSeconds(30), from));
                    Effects.SendTargetParticles(from, 0x375A, 10, 30, 5008, EffectLayer.Waist);
                    break;
                case 4:
                    from.SendMessage("The pie grants you a greater strength effect!");
                    BuffInfo.AddBuff(from, new BuffInfo(BuffIcon.Strength, 1044099, 1153, TimeSpan.FromSeconds(30), from));
                    Effects.SendTargetParticles(from, 0x375A, 10, 30, 5008, EffectLayer.Waist);
                    break;
                case 5:
                    from.SendMessage("The pie grants you a greater dexterity effect!");
                    BuffInfo.AddBuff(from, new BuffInfo(BuffIcon.Bless, 1044100, 1153, TimeSpan.FromSeconds(30), from));
                    Effects.SendTargetParticles(from, 0x375A, 10, 30, 5008, EffectLayer.Waist);
                    break;
            }

            Effects.PlaySound(from.Location, from.Map, 0x1FA); // Sound effect when pie is consumed
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
