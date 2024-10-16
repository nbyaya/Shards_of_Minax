using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a displacer Beast corpse")]
    public class DisplacerBeast : BaseCreature
    {

        private const int MaxFellows = 3;

        private List<Mobile> m_Fellows = new List<Mobile>();
        private Timer m_FellowsTimer;

        [Constructable]
        public DisplacerBeast()
            : base(AIType.AI_OrcScout, FightMode.Closest, 10, 7, 0.2, 0.4)
        {
            Name = "a Displacer Beast";
            Body = 1254;
            Hue = 16385;
            BaseSoundID = 0x462;

            SetStr(61, 85);
            SetDex(86, 105);
            SetInt( 151, 165 );

            SetHits(50, 85);
            SetMana(100);

            SetDamage(8, 16);

            SetDamageType(ResistanceType.Physical, 100);

			SetResistance( ResistanceType.Physical, 35, 65 );
			SetResistance( ResistanceType.Fire, 40, 60 );
			SetResistance( ResistanceType.Cold, 25, 45 );
			SetResistance( ResistanceType.Poison, 40, 60 );
			SetResistance( ResistanceType.Energy, 35, 55 );

            SetSkill(SkillName.MagicResist, 25, 50);
            SetSkill(SkillName.Tactics, 50.1, 65.0);
            SetSkill(SkillName.Wrestling, 50.1, 65.0);


            SetSkill( SkillName.Hiding, 100.0);
            SetSkill( SkillName.Stealth, 125.0 );

            Fame = 10000;
            Karma = -10000;

            VirtualArmor = 20;

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 100;
        
            SetWeaponAbility(WeaponAbility.BleedAttack);
        }

        public DisplacerBeast(Serial serial)
            : base(serial)
        {
        }

        public override bool CanStealth { get { return true; } }
        public override void OnCombatantChange()
        {
            if (Combatant != null && m_FellowsTimer == null)
            {
                m_FellowsTimer = new InternalTimer(this);
                m_FellowsTimer.Start();
            }
        }

        public void CheckFellows()
        {
            if (!Alive || Combatant == null || Map == null || Map == Map.Internal)
            {
                m_Fellows.ForEach(f => f.Delete());
                m_Fellows.Clear();

                m_FellowsTimer.Stop();
                m_FellowsTimer = null;
            }
            else
            {
                for (int i = 0; i < m_Fellows.Count; i++)
                {
                    Mobile friend = m_Fellows[i];

                    if (friend.Deleted)
                        m_Fellows.Remove(friend);
                }

                bool spawned = false;

                for (int i = m_Fellows.Count; i < MaxFellows; i++)
                {
                    BaseCreature friend = new DisplacerBeastImage();

                    friend.MoveToWorld(Map.GetSpawnPosition(Location, 6), Map);
                    friend.Combatant = Combatant;

                    if (friend.AIObject != null)
                        friend.AIObject.Action = ActionType.Combat;

                    m_Fellows.Add(friend);

                    spawned = true;
                }

                if (spawned)
                {
                    PlaySound(0x456);
                }
            }
        }

        private class InternalTimer : Timer
        {
            private DisplacerBeast m_Owner;

            public InternalTimer(DisplacerBeast owner)
                : base(TimeSpan.Zero, TimeSpan.FromSeconds(45.0))
            {
                m_Owner = owner;
            }

            protected override void OnTick()
            {
                m_Owner.CheckFellows();
            }
        }


        public override int Meat
        {
            get
            {
                return 5;
            }
        }
        public override PackInstinct PackInstinct
        {
            get
            {
                return PackInstinct.Feline;
            }
        }
        public override int Hides
        {
            get
            {
                return 5;
            }
        }
        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.Meat;
            }
        }
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager, 2);
        }


        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class DisplacerBeastImage : BaseCreature
    {
        [Constructable]
        public DisplacerBeastImage()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a Displacer Beast";
            Body = 1254;
            Hue = 2594;
            BaseSoundID = 0x462;

            SetStr(61, 85);
            SetDex(86, 105);
            SetInt( 151, 165 );

            SetHits(15, 20);
            SetMana(100);

            SetDamage(8, 16);

            SetDamageType(ResistanceType.Physical, 100);

			SetResistance( ResistanceType.Physical, 35, 65 );
			SetResistance( ResistanceType.Fire, 40, 60 );
			SetResistance( ResistanceType.Cold, 25, 45 );
			SetResistance( ResistanceType.Poison, 40, 60 );
			SetResistance( ResistanceType.Energy, 35, 55 );

            SetSkill(SkillName.MagicResist, 25, 50);
            SetSkill(SkillName.Tactics, 50.1, 65.0);
            SetSkill(SkillName.Wrestling, 50.1, 65.0);

            SetWeaponAbility(WeaponAbility.BleedAttack);         
        }

        public override PackInstinct PackInstinct { get { return PackInstinct.Feline; } }

        public DisplacerBeastImage(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            /*int version = */
            reader.ReadInt();
        }
    }
}