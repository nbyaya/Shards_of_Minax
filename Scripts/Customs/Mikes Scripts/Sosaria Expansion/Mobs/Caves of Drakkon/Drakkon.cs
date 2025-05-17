using System;
using System.Collections.Generic;

using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a paragon dragon corpse")]
	public class Drakkon: BaseSABoss 
	{
	   
        private long _NextAura;

	public override double HealChance 
		{
			get 
			{
				return 1.0;
			}
		}
		private DateTime m_Delay;

		[Constructable]
		public Drakkon() : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.3, 0.5) 
		{
			Name = "Drakkon";
			Title = "Dragon King";
			Body = 826;
			BaseSoundID = 362;
            Hue = 1910;
			
			SetStr(90000);
			SetDex(90000);
			SetInt(90000);

			SetHits(900000000);
			SetStam(431);
			SetMana(90000);

			SetDamage(300, 500);

			SetDamageType(ResistanceType.Physical, 25);
			SetDamageType(ResistanceType.Fire, 50);
			SetDamageType(ResistanceType.Energy, 25);

			SetResistance(ResistanceType.Physical, 90, 90);
			SetResistance(ResistanceType.Fire, 90, 90);
			SetResistance(ResistanceType.Cold, 90, 90);
			SetResistance(ResistanceType.Poison, 90, 90);
			SetResistance(ResistanceType.Energy, 90, 90);

			SetSkill(SkillName.Anatomy, 600.700);
			SetSkill(SkillName.MagicResist, 150.0, 155.0);
			SetSkill(SkillName.Tactics, 120.7, 125.0);
			SetSkill(SkillName.Wrestling, 900.0, 1000.7);
			SetSkill(SkillName.Healing, 500.7, 800.0);
            SetSkill(SkillName.SpiritSpeak, 900.1000);
            SetSkill(SkillName.Necromancy, 500.6, 800.5); 
			SetSkill( SkillName.Tactics, 500.0, 700.0 );
			SetSkill( SkillName.Meditation, 700.0, 800.0 );
			SetSkill( SkillName.Parry, 290.0, 320.0 );
			
			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 60;

			Tamable = false;

			SetWeaponAbility(WeaponAbility.Bladeweave);
			SetWeaponAbility(WeaponAbility.TalonStrike);
			SetSpecialAbility(SpecialAbility.DragonBreath);
		   _NextAura = Core.TickCount + 3000;
		}

		
		
		public Drakkon(Serial serial)
            : base(serial)
        {
        }

        public override bool StatLossAfterTame => true;
        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        
        public override int Meat => 19;
        public override int Hides => 30;
        public override HideType HideType => HideType.Barbed;
        public override int Scales => 7;
        public override ScaleType ScaleType => (Body == 12 ? ScaleType.Yellow : ScaleType.Red);
        public override FoodType FavoriteFood => FoodType.Meat;
        public override bool CanAngerOnTame => true;
        public override bool CanFly => true;
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 4);
            AddLoot(LootPack.Gems, 8);
			
        }


  public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;

            if (combatant == null || combatant.Deleted || combatant.Map != Map || !InRange(combatant, 20) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            {
                DragonExplosion(combatant);
				
            }
        }
		 	
	

        public void DragonExplosion(Mobile m)
        {
            DoHarmful(m);
			Point3D po = m.Location;
			
			         
				
				
			
			Effects.SendLocationEffect(new Point3D(po.X + -1,po.Y + 5, po.Z), Map, 14089, 130, 1909, 0);
			Effects.SendLocationEffect(new Point3D(po.X + 5,po.Y + 2, po.Z), Map, 14089, 130, 1910, 0);
			Effects.SendLocationEffect(new Point3D(po.X + 5,po.Y + -2, po.Z), Map, 14089, 130, 1909, 0);
			Effects.SendLocationEffect(new Point3D(po.X + 1,po.Y + -4, po.Z), Map, 14089, 130, 1910, 0);
			Effects.SendLocationEffect(new Point3D(po.X + 0,po.Y + 2, po.Z), Map, 14089, 130, 1909, 0);
			Effects.SendLocationEffect(new Point3D(po.X + 0,po.Y + -1, po.Z), Map, 14089, 130, 1910, 0);
			Effects.SendLocationEffect(new Point3D(po.X + 3,po.Y + 4, po.Z), Map, 14089, 130, 1909, 0);
			Effects.SendLocationEffect(new Point3D(po.X + 2,po.Y + 2, po.Z), Map, 14089, 130, 1910, 0);
			Effects.SendLocationEffect(new Point3D(po.X + 4,po.Y + -4, po.Z), Map, 14000, 130, 1909, 0);
			Effects.SendLocationEffect(new Point3D(po.X + -6,po.Y + 4, po.Z), Map, 14000, 130, 1910, 0);
			 Effects.SendLocationEffect(new Point3D(po.X + -3,po.Y + 2, po.Z), Map, 14000, 130, 1909, 0);
			 Effects.SendLocationEffect(new Point3D(po.X + -5,po.Y + 0, po.Z), Map, 14000, 130, 1910, 0);
			Effects.SendLocationEffect(new Point3D(po.X + 7,po.Y + 4, po.Z), Map, 14000, 130, 1909, 0);
			 Effects.SendLocationEffect(new Point3D(po.X + 7,po.Y + -2, po.Z), Map, 14000, 130, 1910, 0);
			 Effects.SendLocationEffect(new Point3D(po.X + 3,po.Y + -1, po.Z), Map, 14000, 130, 1909, 0);
			 Effects.SendLocationEffect(new Point3D(po.X + -2,po.Y + 5, po.Z), Map, 14000, 130, 1910, 0);
			 Effects.SendLocationEffect(new Point3D(po.X + -2,po.Y + -1, po.Z), Map, 14000, 130, 1909, 0);
			 Effects.SendLocationEffect(new Point3D(po.X + -2,po.Y + -3, po.Z), Map, 14089, 130, 1910, 0);
			 Effects.SendLocationEffect(new Point3D(po.X + 0,po.Y + 4, po.Z), Map, 14089, 130, 1910, 0);
		}	
		
		public override void OnDamagedBySpell(Mobile caster)
        {
            if (caster == this)
                return;
            if (0.80 >= Utility.RandomDouble())
                spawn(caster); //25% chance to spawn a minion of Scelestus when hit by a spell
        }

        public void spawn(Mobile m)
        {
            Map map = this.Map;

            if (map == null)
                return;

            BaseCreature spawned;
            spawned = new MinionOfScelestus(); //Minion of Scelestus is a demon that has 30k hp
			spawned.Name = "Stygian Minion";
			spawned.DamageMin = 300;
			spawned.DamageMax = 400;
			spawned.HitsMaxSeed = Utility.RandomMinMax(64000,66000);
			spawned.Hits = spawned.HitsMaxSeed;
            Team = this.Team;
		
		bool validLocation = false;
            Point3D loc = this.Location;

            for (int j = 0; !validLocation && j < 10; ++j)
            {
                int x = X + Utility.Random(3) - 1;
                int y = Y + Utility.Random(3) - 1;
                int z = map.GetAverageZ(x, y);

                if (validLocation = map.CanFit(x, y, this.Z, 16, false, false))
                    loc = new Point3D(x, y, Z);
                else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                    loc = new Point3D(x, y, z);
         }
		
		 if (validLocation)
            {
                spawned.MoveToWorld(loc, map);
                spawned.Combatant = m;
        }    }
		
		public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (from is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)from;

                if (0.7 >= Utility.RandomDouble())
                    bc.Damage(damage, this); //Reflects damage back to pets
            }
		}

		public override Type[] UniqueSAList 
		{
			get 
			{
				return new Type[] 
				{
					typeof(BurningAmber),
					typeof(DraconisWrath),
					typeof(DragonHideShield),
					typeof(FallenMysticsSpellbook),
					typeof(LifeSyphon),
					typeof(GargishSignOfOrder),
					typeof(HumanSignOfOrder),
					typeof(VampiricEssence)
				};
			}
		}
		public override Type[] SharedSAList 
		{
			get 
			{
				return new Type[] 
				{
					typeof(AxesOfFury),
					typeof(SummonersKilt),
					typeof(GiantSteps),
					typeof(StoneDragonsTooth),
					typeof(TokenOfHolyFavor)
				};
			}
		}

		public override bool AlwaysMurderer 
		{
			get 
			{
				return true;
			}
		}
		public override bool Unprovokable 
		{
			get 
			{
				return false;
			}
		}
		public override bool BardImmune 
		{
			get 
			{
				return false;
			}
		}
		public override int DragonBlood 
		{
			get 
			{
				return 48;
			}
		}
		public override bool CanFlee 
		{
			get {
				return false;
			}
		}

		public override void OnThink() 
		{
			base.OnThink();

			if (Combatant == null || !(Combatant is Mobile)) return;

			if (DateTime.UtcNow > m_Delay) 
			{
				switch (Utility.Random(4)) 
				{
					case 0:
						CrimsonMeteor(this, (Mobile) Combatant, 70, 125);
						break;
					case 1:
						DoStygianFireball();
						break;
					case 2:
						DoFireColumn();
						break;
					case 3:
						DoFocusedLeech((Mobile) Combatant, "King Yakkuza Have Mercy Of Theyr Soul!");
						break;
				}

				m_Delay = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 11));
			}
		}

		public override void OnDeath(Container c) 
		{
			base.OnDeath(c);

			

			if (Paragon.ChestChance > Utility.RandomDouble()) c.DropItem(new ParagonChest(Name, 5));
		}

	

		public override void Serialize(GenericWriter writer) 
		{
			base.Serialize(writer);
			writer.Write((int) 1);
		}

		public override void Deserialize(GenericReader reader) 
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}

		#region Crimson Meteor
		public static void CrimsonMeteor(Mobile owner, Mobile combatant, int minDamage, int maxDamage) 
		{
			if (!combatant.Alive || combatant.Map == null || combatant.Map == Map.Internal) return;

			new CrimsonMeteorTimer(owner, combatant.Location, 70, 80).Start();
		}

		public class CrimsonMeteorTimer: Timer 
		{
			private Mobile m_From;
			private Map m_Map;
			private int m_Count;
			private int m_MaxCount;
			private bool m_DoneDamage;
			private Point3D m_LastTarget;
			private Rectangle2D m_ShowerArea;
			private List < Mobile > m_ToDamage;

			private int m_MinDamage,
			m_MaxDamage;

			public CrimsonMeteorTimer(Mobile from, Point3D loc, int min, int max) : base(TimeSpan.FromMilliseconds(20.0), TimeSpan.FromMilliseconds(20.0)) 
			{
				m_From = from;
				m_Map = from.Map;
				m_Count = 0;
				m_MaxCount = 90; // in ticks
				m_LastTarget = loc;
				m_DoneDamage = false;
				m_ShowerArea = new Rectangle2D(loc.X - 2, loc.Y - 2, 4, 4);

				m_MinDamage = min;
				m_MaxDamage = max;

				m_ToDamage = new List < Mobile > ();

				IPooledEnumerable eable = m_Map.GetMobilesInBounds(m_ShowerArea);

				foreach(Mobile m in eable) 
				{
					if (m != from && m_From.CanBeHarmful(m)) m_ToDamage.Add(m);
				}

				eable.Free();
			}

			protected override void OnTick() 
			{
				if (m_From == null || m_From.Deleted || m_Map == null || m_Map == Map.Internal) 
				{
					Stop();
					return;
				}

				if (0.33 > Utility.RandomDouble()) 
				{
					var field = new FireField(m_From, 25, Utility.RandomBool());
					field.MoveToWorld(m_LastTarget, m_Map);
				}

				Point3D start = new Point3D();
				Point3D finish = new Point3D();

				finish.X = m_ShowerArea.X + Utility.Random(m_ShowerArea.Width);
				finish.Y = m_ShowerArea.Y + Utility.Random(m_ShowerArea.Height);
				finish.Z = m_From.Z;

				SpellHelper.AdjustField(ref finish, m_Map, 16, false);

				//objects move from upper right/right to left as per OSI
				start.X = finish.X + Utility.RandomMinMax( - 4, 4);
				start.Y = finish.Y - 15;
				start.Z = finish.Z + 50;

				Effects.SendMovingParticles( new Entity(Serial.Zero, start, m_Map), new Entity(Serial.Zero, finish, m_Map), 0x36D4, 15, 0, false, false, 0, 0, 9502, 1, 0, (EffectLayer) 255, 0x100);

				Effects.PlaySound(finish, m_Map, 0x11D);

				m_LastTarget = finish;
				m_Count++;

				if (m_Count >= m_MaxCount / 2 && !m_DoneDamage) 
				{
					if (m_ToDamage != null && m_ToDamage.Count > 0) 
					{
						int damage;

						foreach(Mobile mob in m_ToDamage) 
						{
							damage = Utility.RandomMinMax(m_MinDamage, m_MaxDamage);

							m_From.DoHarmful(mob);
							AOS.Damage(mob, m_From, damage, 0, 10000, 0, 0, 0);

							mob.FixedParticles(0x36BD, 1, 15, 9502, 0, 3, (EffectLayer) 255);
						}
					}

					m_DoneDamage = true;
					return;
				}

				if (m_Count >= m_MaxCount) Stop();
			}
		}
		#endregion

		#region Fire Column
		public void DoFireColumn() 
		{
			var map = Map;

			if (map == null) return;

			Direction columnDir = Utility.GetDirection(this, Combatant);

			Packet flash = ScreenLightFlash.Instance;
			IPooledEnumerable e = Map.GetClientsInRange(Location, Core.GlobalUpdateRange);

			foreach(NetState ns in e) 
			{
				if (ns.Mobile != null) ns.Mobile.Send(flash);
			}

			e.Free();

			int x = X;
			int y = Y;
			bool south = columnDir == Direction.East || columnDir == Direction.West;

			Movement.Movement.Offset(columnDir, ref x, ref y);
			Point3D p = new Point3D(x, y, Z);
			SpellHelper.AdjustField(ref p, map, 16, false);

			var fire = new FireField(this, Utility.RandomMinMax(25, 32), south);
			fire.MoveToWorld(p, map);

			for (int i = 0; i < 7; i++) 
			{
				Movement.Movement.Offset(columnDir, ref x, ref y);

				p = new Point3D(x, y, Z);
				SpellHelper.AdjustField(ref p, map, 16, false);

				fire = new FireField(this, Utility.RandomMinMax(25, 32), south);
				fire.MoveToWorld(p, map);
			}
		}
		#endregion

		#region Fire Field
		public class FireField: Item 
		{
			private Mobile m_Owner;
			private Timer m_Timer;
			private DateTime m_Destroy;

			[Constructable]
			public FireField(Mobile owner, int duration, bool south) : base(GetItemID(south)) 
			{
				Movable = false;
				m_Destroy = DateTime.UtcNow + TimeSpan.FromSeconds(duration);

				m_Owner = owner;
				m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), new TimerCallback(OnTick));
			}

			private static int GetItemID(bool south) 
			{
				if (south) return 0x398C;
				else return 0x3996;
			}

			public override void OnAfterDelete() 
			{
				if (m_Timer != null) m_Timer.Stop();
			}

			private void OnTick() 
			{
				if (DateTime.UtcNow > m_Destroy) 
				{
					Delete();
				}
				else 
				{
					IPooledEnumerable eable = GetMobilesInRange(0);
					List < Mobile > list = new List < Mobile > ();

					foreach(Mobile m in eable) 
					{
						if (m == null) 
						{
							continue;
						}

						if (m_Owner == null || CanTargetMob(m)) 
						{
							list.Add(m);
						}
					}

					eable.Free();

					foreach(var mob in list) 
					{
						DealDamage(mob);
					}

					ColUtility.Free(list);
				}
			}

			public override bool OnMoveOver(Mobile m) 
			{
				DealDamage(m);

				return true;
			}

			public void DealDamage(Mobile m) 
			{
				if (m != m_Owner && (m_Owner == null || CanTargetMob(m))) AOS.Damage(m, m_Owner, Utility.RandomMinMax(2, 4), 0, 100, 0, 0, 0);
			}

			public bool CanTargetMob(Mobile m) 
			{
				return m != m_Owner && m_Owner.CanBeHarmful(m, false) && (m is PlayerMobile || (m is BaseCreature && ((BaseCreature) m).GetMaster() is PlayerMobile));
			}

			public FireField(Serial serial) : base(serial) {}

			public override void Serialize(GenericWriter writer) 
			{
				// Unsaved.
			}

			public override void Deserialize(GenericReader reader) {}
		}
		#endregion

		#region Stygian Fireball
		public void DoStygianFireball() 
		{
			if (! (Combatant is Mobile) || !InRange(Combatant.Location, 10)) return;

			new StygianFireballTimer(this, (Mobile) Combatant);
			PlaySound(0x1F3);
		}

		private class StygianFireballTimer: Timer 
		{
			private Drakkon m_Dragon;
			private Mobile m_Combatant;
			private int m_Ticks;

			public StygianFireballTimer(Drakkon dragon, Mobile combatant) : base(TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(200))
			{
				m_Dragon = dragon;
				m_Combatant = combatant;
				m_Ticks = 0;
				Start();
			}

			protected override void OnTick() 
			{
				m_Dragon.MovingParticles(m_Combatant, 0x46E6, 7, 0, false, true, 1265, 0, 9502, 4019, 0x026, 0);

				if (m_Ticks >= 10) 
				{
					int damage = Utility.RandomMinMax(12000, 15000);

					Timer.DelayCall(TimeSpan.FromSeconds(.3), new TimerStateCallback(DoDamage_Callback), new object[] 
					{
						m_Combatant,
						m_Dragon,
						damage
					});

					Stop();
				}

				m_Ticks++;
			}

			public void DoDamage_Callback(object state) 
			{
				object[] obj = (object[]) state;

				Mobile c = (Mobile) obj[0];
				Mobile d = (Mobile) obj[1];
				int dam = (int) obj[2];

				d.DoHarmful(c);
				AOS.Damage(c, d, dam, false, 0, 0, 0, 0, 0, 100, 0, false);
			}
		}
		#endregion

		#region hp drain
		private void DoFocusedLeech(Mobile combatant, string message) 
		{
			this.Say(true, message);

			Timer.DelayCall(TimeSpan.FromSeconds(0.3), new TimerStateCallback(DoFocusedLeech_Stage1), combatant);
		}

		private void DoFocusedLeech_Stage1(object state) 
		{
			Mobile combatant = (Mobile) state;

			if (this.CanBeHarmful(combatant)) 
			{
				this.MovingParticles(combatant, 0x36FA, 1, 0, false, false, 1108, 0, 9533, 1, 0, (EffectLayer) 255, 0x100);
				this.MovingParticles(combatant, 0x0001, 1, 0, false, true, 1108, 0, 9533, 9534, 0, (EffectLayer) 255, 0);
				this.PlaySound(0x1FB);

				Timer.DelayCall(TimeSpan.FromSeconds(0.5), new TimerStateCallback(DoFocusedLeech_Stage2), combatant);
			}
		}

		private void DoFocusedLeech_Stage2(object state) 
		{
			Mobile combatant = (Mobile) state;

			if (this.CanBeHarmful(combatant)) 
			{
				combatant.MovingParticles(this, 0x36F4, 1, 0, false, false, 32, 0, 9535, 1, 0, (EffectLayer) 255, 0x100);
				combatant.MovingParticles(this, 0x0001, 1, 0, false, true, 32, 0, 9535, 9536, 0, (EffectLayer) 255, 0);

				this.PlaySound(0x209);
				this.DoHarmful(combatant);
				this.Hits += AOS.Damage(combatant, this, Utility.RandomMinMax(4000, 8000) - (Core.AOS ? 0 : 10), 100, 0, 0, 0, 0);
			}
		}
		#endregion
	}
}