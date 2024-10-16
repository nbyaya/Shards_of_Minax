using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.CampingMagic
{
    public class HerbalRemedy : CampingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Herbal Remedy", "Gather and Cure",
            // SkillLevel
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; } // Arbitrary choice; adjust as needed
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 20; } }

        public HerbalRemedy(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private HerbalRemedy m_Owner;

            public InternalTarget(HerbalRemedy owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D)
                    m_Owner.Target((IPoint3D)targeted);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, new Point3D(p)); // Convert IPoint3D to Point3D

                List<Item> nearbyHerbs = FindHerbs(new Point3D(p), Caster.Map, 5); // Finds herbs within a 5-tile radius

                if (nearbyHerbs.Count > 0)
                {
                    Effects.PlaySound(new Point3D(p), Caster.Map, 0x44); // Sound effect for gathering herbs

                    foreach (Item herb in nearbyHerbs)
                    {
                        herb.Delete(); // Remove the herb item from the world
                    }

                    BasePotion potion = new CurePotion(); // Create a potion (e.g., Cure Potion)
                    Caster.AddToBackpack(potion); // Add the potion to the caster's backpack

                    Caster.SendMessage("You gather herbs and create a potion to cure poison or diseases!");

                    Effects.SendLocationParticles(EffectItem.Create(new Point3D(p), Caster.Map, EffectItem.DefaultDuration), 0x373A, 1, 30, 1153, 2, 9962, 0); // Flashy effect
                }
                else
                {
                    Caster.SendMessage("There are no herbs nearby to gather.");
                }
            }

            FinishSequence();
        }

        private List<Item> FindHerbs(Point3D p, Map map, int range)
        {
            List<Item> herbs = new List<Item>();

            foreach (Item item in map.GetItemsInRange(p, range))
            {
                if (item is Herb) // Assuming Herb is a defined item type
                {
                    herbs.Add(item);
                }
            }

            return herbs;
        }

    }

    public class Herb : Item
    {
        [Constructable]
        public Herb() : base(0x18E2) // Example Item ID for Herb
        {
            Name = "Herb";
            Hue = Utility.RandomGreenHue();
            Stackable = true;
        }

        public Herb(Serial serial) : base(serial)
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

	public class CurePotion : BasePotion
	{
		[Constructable]
		public CurePotion() : base(0xF0B) // Use the ID for Cure Potion, or adjust based on your definitions
		{
		}

		public CurePotion(Serial serial) : base(serial)
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

		public override void Drink(Mobile from)
		{
			// Example effect: Cure the target from poison
			from.CurePoison(null); // Cure the poison on the mobile
			from.SendMessage("You feel better as the potion takes effect."); // Feedback message
		}
	}

}
