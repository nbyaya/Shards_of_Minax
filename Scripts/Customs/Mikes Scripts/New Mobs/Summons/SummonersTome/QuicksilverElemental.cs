using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a quicksilver elemental's corpse")]
    public class QuicksilverElemental : BaseCreature
    {


        [Constructable]
        public QuicksilverElemental()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a quicksilver elemental";
            Body = 13; // Use water elemental body (can be changed)
            Hue = 1150; // Shiny silver hue
            BaseSoundID = 278;


            double scale = 0;

            int str = (int)(300 + (200 * scale));
            int dex = (int)(250 + (100 * scale));
            int intel = (int)(200 + (100 * scale));

            SetStr(str);
            SetDex(dex);
            SetInt(intel);

            SetHits((int)(400 + (300 * scale)));

            SetDamage((int)(20 + (10 * scale)), (int)(30 + (10 * scale)));

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 40 + (int)(30 * scale));
            SetResistance(ResistanceType.Fire, 20 + (int)(20 * scale));
            SetResistance(ResistanceType.Cold, 30 + (int)(20 * scale));
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50 + (int)(30 * scale));

            SetSkill(SkillName.MagicResist, 90.0 + (20.0 * scale));
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0 + (20.0 * scale));
            SetSkill(SkillName.Meditation, 100.0);


            Fame = 16000;
            Karma = -16000;

            VirtualArmor = 75;

            PackItem(new BlackPearl(3));
        }

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override bool AutoDispel => true;
        public override bool Unprovokable => true;

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            // Chance to phase out (untargetable) for 2 seconds
            if (Utility.RandomDouble() < 0.1)
            {
                Hidden = true;
                Effects.PlaySound(Location, Map, 0x10B);
                Timer.DelayCall(TimeSpan.FromSeconds(2), () => Hidden = false);
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            // 25% chance to deflect melee attacks
            if (Utility.RandomDouble() < 0.25)
            {
                attacker.SendMessage("Your blow glances off the quicksilver surface!");
                attacker.PlaySound(0x28E);
                Combatant = attacker;
                Damage(0, attacker); // No damage
            }
        }

        public QuicksilverElemental(Serial serial) : base(serial) { }

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
