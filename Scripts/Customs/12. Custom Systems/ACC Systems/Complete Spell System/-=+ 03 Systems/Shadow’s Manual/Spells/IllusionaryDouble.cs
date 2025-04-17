using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;
using Server;

namespace Server.ACC.CSS.Systems.HidingMagic
{
    public class IllusionaryDoubleSpell : HidingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Illusionary Double", "Illusionem Gemina",
            21011, // Animation ID
            9210,  // Sound ID
            false
        );

        public override SpellCircle Circle => SpellCircle.Fourth;
        public override double CastDelay => 0.2;
        public override double RequiredSkill => 50.0;
        public override int RequiredMana => 25;

        public IllusionaryDoubleSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You create an illusionary double of yourself!");

                Effects.SendLocationParticles(
                    EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration),
                    0x376A, 1, 29, 1153, 0, 5029, 0
                );
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1FD);

                IllusionaryDouble illusion = new IllusionaryDouble(Caster);
                illusion.MoveToWorld(Caster.Location, Caster.Map);

                Timer.DelayCall(TimeSpan.FromSeconds(10.0), illusion.Delete);
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }

    public class IllusionaryDouble : Mobile
    {
        private Mobile m_Caster;

        // Required for deserialization
        public IllusionaryDouble(Serial serial) : base(serial)
        {
        }

        public IllusionaryDouble(Mobile caster)
        {
            m_Caster = caster;

            Body = caster.Body;
            Hue = caster.Hue;
            Name = caster.Name;
            Female = caster.Female;

            Timer.DelayCall(TimeSpan.FromSeconds(10.0), Delete);

            for (int i = 0; i < caster.Items.Count; ++i)
            {
                Item item = caster.Items[i];

                if (item.Layer != Layer.Backpack && item.Layer != Layer.Mount && item.Layer != Layer.Bank)
                {
                    Item clone = new Item(item.ItemID)
                    {
                        Layer = item.Layer,
                        Movable = false,
                        Hue = item.Hue
                    };

                    AddItem(clone);
                }
            }

            AggroNearbyEnemies();
        }

        private void AggroNearbyEnemies()
        {
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m.Combatant == m_Caster)
                {
                    m.Combatant = this;
                    m.SendMessage("You are distracted by an illusion!");
                }
            }
        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            if (m_Caster != null)
                m_Caster.SendMessage("Your illusionary double vanishes.");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version

            writer.Write(m_Caster);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Caster = reader.ReadMobile();
        }
    }
}
