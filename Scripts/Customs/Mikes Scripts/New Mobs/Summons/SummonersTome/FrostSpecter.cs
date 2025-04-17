using System;
using Server.Mobiles;
using Server.Spells;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a frost specter corpse")]
    public class FrostSpecter : BaseCreature
    {
        private Mobile m_Summoner;

        [Constructable]
        public FrostSpecter()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {


            Name = "a frost specter";
            Body = 310; // Wraith-like form
            Hue = 0x480; // Frosty blue/white hue
            BaseSoundID = 0x482;

            int spiritSpeak = 0;

            int str = 100 + (spiritSpeak / 2);
            int dex = 80 + (spiritSpeak / 3);
            int intel = 100 + (spiritSpeak / 2);

            SetStr(str);
            SetDex(dex);
            SetInt(intel);

            SetHits(str);

            SetDamage(12 + (spiritSpeak / 20), 18 + (spiritSpeak / 15));

            SetDamageType(ResistanceType.Cold, 100);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 80, 100);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 80.0 + (spiritSpeak / 2));
            SetSkill(SkillName.Tactics, 70.0 + (spiritSpeak / 3));
            SetSkill(SkillName.Wrestling, 75.0 + (spiritSpeak / 3));
            SetSkill(SkillName.EvalInt, 60.0 + (spiritSpeak / 4));
            SetSkill(SkillName.Magery, 60.0 + (spiritSpeak / 4));

            Fame = 3000;
            Karma = -3000;

            VirtualArmor = 30;

            // Special Weapon Ability
            SetWeaponAbility(WeaponAbility.ParalyzingBlow);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && Utility.RandomDouble() < 0.05)
            {
                TryTeleport();
            }
        }

		private void TryTeleport()
		{
			if (Combatant == null || !Combatant.Alive)
				return;

			Map map = this.Map;

			if (map == null)
				return;

			int x = Combatant.X + Utility.RandomMinMax(-1, 1);
			int y = Combatant.Y + Utility.RandomMinMax(-1, 1);
			int z = map.GetAverageZ(x, y);

			Point3D to = new Point3D(x, y, z);

			if (map.CanFit(to, 16, false, false))
			{
				Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023, 0, 5023, 0);
				MoveToWorld(to, map);
				Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023, 0, 5023, 0);

				if (Combatant is Mobile target)
					target.SendMessage("You feel a sudden chill as the specter phases behind you!");
			}
		}


        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.2)
            {
                defender.Freeze(TimeSpan.FromSeconds(2));
                defender.SendMessage("You are frozen by the Frost Specter’s chilling grasp!");
                Effects.SendTargetEffect(defender, 0x376A, 10, 20);
            }
        }

        public FrostSpecter(Serial serial) : base(serial)
        {
        }

        public override bool BleedImmune => true;
        public override bool AutoDispel => true; // Dispel weaker summons

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
