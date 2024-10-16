using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Nostradamus")]
    public class UltimateMasterMysticism : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterMysticism()
            : base(AIType.AI_Mage)
        {
            Name = "Nostradamus";
            Title = "The Seer of Providence";
            Body = 0x190;
            Hue = 0x4001;

            SetStr(305, 425);
            SetDex(72, 150);
            SetInt(505, 750);

            SetHits(12000);
            SetMana(2500);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Mysticism, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;
			
            AddItem(new Robe(Utility.RandomBlueHue()));
            AddItem(new WizardsHat(Utility.RandomBlueHue()));
            AddItem(new Sandals());

            HairItemID = 0x203B; // Short Hair
            HairHue = 0x386; // Gray
        }

        public UltimateMasterMysticism(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(CrystalBall), typeof(SeersRobes) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(ProphecyScroll), typeof(MysticShieldScroll) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(MysticTome), typeof(PropheticStaff) }; }
        }

        public override MonsterStatuetteType[] StatueTypes
        {
            get { return new MonsterStatuetteType[] { }; }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 6);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new PowerScroll(SkillName.Mysticism, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new CrystalBall());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new SeersRobes());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: Prophecy(defender); break;
                    case 1: MysticShield(); break;
                    case 2: FutureSight(defender); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void Prophecy(Mobile defender)
        {
            if (defender != null)
            {
                defender.SendLocalizedMessage(1070853); // You have been cursed by a dark prophecy!

                StatMod mod = new StatMod(StatType.All, "Prophecy", -30, TimeSpan.FromSeconds(60.0));
                defender.AddStatMod(mod);

                defender.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                defender.PlaySound(0x1EA);
            }
        }

        public void MysticShield()
        {
            FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
            PlaySound(0x1ED);

            AddStatMod(new StatMod(StatType.All, "MysticShield", 20, TimeSpan.FromSeconds(60.0)));

            this.Say("I am protected by the mystic forces!");
        }

        public void FutureSight(Mobile defender)
        {
            if (defender != null)
            {
                defender.SendLocalizedMessage(1070854); // Nostradamus foresees your attack and evades it!

                for (int i = 0; i < 3; i++)
                {
                    Point3D to = defender.Location;

                    for (int j = 0; j < 10; j++)
                    {
                        Point3D from = new Point3D(to.X + Utility.RandomMinMax(-1, 1), to.Y + Utility.RandomMinMax(-1, 1), to.Z);
                        Effects.SendMovingParticles(new Entity(Serial.Zero, from, this.Map), new Entity(Serial.Zero, to, this.Map), 0x36D4, 7, 0, false, false, 0, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                    }
                }

                this.Combatant = null;
            }
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