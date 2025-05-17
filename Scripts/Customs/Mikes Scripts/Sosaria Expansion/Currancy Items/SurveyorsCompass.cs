using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    public class SurveyorsCompass : Item
    {
        [Constructable]
        public SurveyorsCompass() : base(0x186B)
        {
            Name = "Surveyor's Compass";
            Hue = 2118;
            Weight = 1.0;
        }

        public SurveyorsCompass(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack to use it.
                return;
            }

            from.SendMessage("Select a magic map to remeasure its danger radius.");
            from.Target = new CompassTarget(this);
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Remeasures the map's radius of influence.");
        }

        private class CompassTarget : Target
        {
            private readonly SurveyorsCompass _compass;

            public CompassTarget(SurveyorsCompass compass) : base(12, false, TargetFlags.None)
            {
                _compass = compass;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (_compass.Deleted)
                    return;

                if (targeted is MagicMapBase map)
                {
                    if (!map.IsChildOf(from.Backpack))
                    {
                        from.SendMessage("That map must be in your backpack.");
                        return;
                    }

                    int oldRadius = map.SpawnRadius;
                    int newRadius = Utility.RandomMinMax(20, 120);

                    map.SpawnRadius = newRadius;
                    from.SendMessage($"The map's radius has been recalibrated: {oldRadius} → {newRadius}.");

                    _compass.Delete();
                }
                else
                {
                    from.SendMessage("That is not a valid target for the compass.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                if (!_compass.Deleted)
                    from.SendMessage("You put the compass away.");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
