using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.MageryMagic
{
    public class SummonFamiliar : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Summon Familiar", "Kal Vas Xen",
            21004,
            9300,
            false,
            Reagent.Bloodmoss,
            Reagent.MandrakeRoot,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 40; } }

        public SummonFamiliar(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                try
                {
                    Type familiarType = GetRandomFamiliarType();
                    BaseCreature familiar = (BaseCreature)Activator.CreateInstance(familiarType);
                    SpellHelper.Summon(familiar, Caster, 0x215, TimeSpan.FromSeconds(2.0 * Caster.Skills[CastSkill].Value), false, false);

                    // Play flashy visual and sound effects
                    Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x213);

                    Caster.SendMessage("You have summoned a magical familiar to assist you!");
                }
                catch
                {
                    Caster.SendMessage("The spell fizzles and fails.");
                }
            }

            FinishSequence();
        }

        private Type GetRandomFamiliarType()
        {
            Type[] familiars = new Type[]
            {
                typeof(WolfFamiliar),
                typeof(CatFamiliar),
                typeof(RandomFey),
                typeof(RandomRodent)
            };

            return familiars[Utility.Random(familiars.Length)];
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }

    public class WolfFamiliar : BaseCreature
    {
        public WolfFamiliar() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "a wolf familiar";
            Body = 0x25; // Body ID for a wolf
            Hue = Utility.RandomList(0x497, 0x4F2); // Random hue
            BaseSoundID = 0xE5;

            SetStr(50);
            SetDex(70);
            SetInt(30);

            SetHits(40);
            SetMana(0);

            SetDamage(5, 10);

            SetSkill(SkillName.MagicResist, 20.0);
            SetSkill(SkillName.Tactics, 50.0);
            SetSkill(SkillName.Wrestling, 50.0);

            Fame = 300;
            Karma = 300;

            VirtualArmor = 12;

            Tamable = false;
            ControlSlots = 1;
        }

        public WolfFamiliar(Serial serial) : base(serial)
        {
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

    public class CatFamiliar : BaseCreature
    {
        public CatFamiliar() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "a cat familiar";
            Body = 0xC9; // Body ID for a cat
            Hue = Utility.RandomList(0x497, 0x4F2); // Random hue
            BaseSoundID = 0x69;

            SetStr(40);
            SetDex(80);
            SetInt(30);

            SetHits(30);
            SetMana(0);

            SetDamage(4, 8);

            SetSkill(SkillName.MagicResist, 20.0);
            SetSkill(SkillName.Tactics, 40.0);
            SetSkill(SkillName.Wrestling, 40.0);

            Fame = 300;
            Karma = 300;

            VirtualArmor = 8;

            Tamable = false;
            ControlSlots = 1;
        }

        public CatFamiliar(Serial serial) : base(serial)
        {
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

    // Similar classes for BearFamiliar and FalconFamiliar can be defined here...
}
