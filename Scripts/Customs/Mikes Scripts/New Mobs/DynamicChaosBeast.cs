using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Mobiles
{
    public class DynamicChaosBeast : BaseCreature
    {
        [Constructable]
        public DynamicChaosBeast() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Dynamic Chaos Beast";
            Body = 774; // This is just an example body, adjust as needed
            BaseSoundID = 357;
            Hue = Utility.RandomList(0x480, 0x482, 0x485, 0x487); // Random hues for variety

            SetStr(300, 400);
            SetDex(75, 95);
            SetInt(250, 300);

            SetHits(300, 400);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Magery, 80.0, 100.0);
            SetSkill(SkillName.Meditation, 90.0, 100.0);
            SetSkill(SkillName.EvalInt, 80.0, 100.0);
            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 70.0, 90.0);
            SetSkill(SkillName.Wrestling, 65.0, 85.0);

            Fame = 10000;
            Karma = -10000;

            VirtualArmor = 40;

            AddItem(new LightSource()); // Adds an ambient light effect for visibility in dark areas

            Timer.DelayCall(TimeSpan.FromSeconds(5), ChangeAttributes); // Call attribute change every 5 seconds
        }

        public DynamicChaosBeast(Serial serial) : base(serial)
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

        private void ChangeAttributes()
        {
            // Change the beast's resistances randomly
            SetResistance(ResistanceType.Physical, Utility.RandomMinMax(20, 80));
            SetResistance(ResistanceType.Fire, Utility.RandomMinMax(20, 80));
            SetResistance(ResistanceType.Cold, Utility.RandomMinMax(20, 80));
            SetResistance(ResistanceType.Poison, Utility.RandomMinMax(20, 80));
            SetResistance(ResistanceType.Energy, Utility.RandomMinMax(20, 80));

            // Change damage type
            int phys = Utility.Random(100);
            SetDamageType(ResistanceType.Physical, phys);
            SetDamageType(ResistanceType.Energy, 100 - phys);

            // Change appearance for visual feedback on changes
            Body = Utility.RandomList(774, 775, 776); // Example bodies, adjust as needed
            Hue = Utility.RandomList(0x480, 0x482, 0x485, 0x487);

            Say("The chaos within me shifts!"); // Emote change to players

            Timer.DelayCall(TimeSpan.FromSeconds(5), ChangeAttributes); // Continue the loop
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && Utility.RandomDouble() < 0.1) // 10% chance to cast a spell
            {
                SpecialAttack();
            }
        }

		private void SpecialAttack()
		{
			switch (Utility.Random(3))
			{
				case 0: // Chaos Wave
					Say("Feel the chaos!");
					AOS.Damage(Combatant, this, Utility.RandomMinMax(20, 40), 0, 0, 0, 0, 100); // All energy damage
					break;
				case 1: // Energy Vortex
					Say("Energy vortex forms around you!");
					new EnergyVortex().MoveToWorld(Combatant.Location, Combatant.Map);
					break;
				case 2: // Random Teleport
					Say("I am everywhere!");
					Map map = Map;
					if (map != null)
					{
						for (int i = 0; i < 5; i++) // Try to teleport 5 times
						{
							// Define a small area around the combatant for random spawn point
							Rectangle2D area = new Rectangle2D(Combatant.X - 10, Combatant.Y - 10, 20, 20);
							Point3D newLocation = map.GetRandomSpawnPoint(area);
							
							// Check if the new location is passable, using 0 for Z if not specified
							if (map.CanFit(newLocation.X, newLocation.Y, newLocation.Z, this.Body, false, false, true))
							{
								Location = newLocation;
								break;
							}
						}
					}
					break;
			}
		}
    }
}