using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Carmen Sandiego")]
    public class UltimateMasterSnooping : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterSnooping()
            : base(AIType.AI_Melee)
        {
            Name = "Carmen Sandiego";
            Title = "The Master Thief";
            Body = 0x191;
            Hue = 0x8403;

            SetStr(305, 425);
            SetDex(152, 200);
            SetInt(505, 750);

            SetHits(10000);
            SetMana(2500);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Poison, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Stealth, 120.0);
            SetSkill(SkillName.Snooping, 120.0);
            SetSkill(SkillName.Hiding, 120.0);
            SetSkill(SkillName.Lockpicking, 120.0);
            SetSkill(SkillName.Fencing, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 60;

            AddItem(new Cloak(Utility.RandomRedHue()));
            AddItem(new Boots(Utility.RandomRedHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new FancyShirt(Utility.RandomBlueHue()));
			AddItem(new WideBrimHat(Utility.RandomRedHue()));

            HairItemID = 0x2048; // Long Hair
            HairHue = 0x47E;
        }

        public UltimateMasterSnooping(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(ThiefsGloves), typeof(DisguiseKit) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(LockpickSet), typeof(SnoopingTome) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(CarmenStatue), typeof(InvisibleInk) }; }
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

            c.DropItem(new PowerScroll(SkillName.Snooping, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new ThiefsGloves());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new DisguiseKit());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: StealthMove(); break;
                    case 1: SleightOfHand(defender); break;
                    case 2: EscapeArtist(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void StealthMove()
        {
            this.Hidden = true;
            this.SendLocalizedMessage(1042550); // You have disappeared!
            this.PlaySound(0x22F);
            this.FixedParticles(0x373A, 10, 15, 5036, EffectLayer.Waist);
        }

        public void SleightOfHand(Mobile defender)
        {
            if (defender != null)
            {
                defender.SendLocalizedMessage(1060215); // You feel a sharp tug as something is stolen from you!
                this.DoHarmful(defender);
                // Simulate item steal - in actual implementation, a random item could be transferred to Carmen's inventory.
            }
        }

        public void EscapeArtist()
        {
            this.Hidden = true;
            this.SendLocalizedMessage(1042550); // You have disappeared!
            this.PlaySound(0x22F);
            this.FixedParticles(0x373A, 10, 15, 5036, EffectLayer.Waist);
            this.Heal(Utility.RandomMinMax(50, 100)); // Heals Carmen for a random amount between 50 and 100.
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
