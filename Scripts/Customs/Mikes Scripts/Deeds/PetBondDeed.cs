using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using System.Collections;
using Server.Gumps;
using Server.Targeting;
using Server.Misc;
using Server.Accounting;
using System.Xml;
using Server.Mobiles; 

namespace Server.Items
{
	public class PetBondDeed : Item
	{
		[Constructable]
		public PetBondDeed() : base( 0x14F0 )
		{
			base.Weight = 0;
			base.Name = "a pet bond deed";
		}		

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) )
			{
				from.Target = new InternalTarget(from, this);
			}
			else
			{
				from.SendMessage("� necess�rio estar em sua mochila.");
			}
		}
		
		public PetBondDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
		
	}
	
		public class InternalTarget : Target
		{
			private Mobile m_From;
			private PetBondDeed m_Deed;
			
			public InternalTarget( Mobile from, PetBondDeed deed ) :  base ( 3, false, TargetFlags.None )
			{
				m_Deed = deed;
				m_From = from;
				from.SendMessage("Selecione o animal para bondar");
		
				
			}
			
			protected override void OnTarget( Mobile from, object targeted )
			{
				
				if (m_Deed.IsChildOf( m_From.Backpack ) )
				{					
					if ( targeted is Mobile )
					{
						if ( targeted is BaseCreature )
						{
							BaseCreature creature = (BaseCreature)targeted;
							if( !creature.Tamable ){
								from.SendMessage("Este animal n�o esta domado");
							}
							else if(  !creature.Controlled || creature.ControlMaster != from ){
								from.SendMessage("Este animal n�o � seu");
							}
							else if( creature.IsDeadPet ){
								from.SendMessage("Este animal esta morto");
							}
							else if ( creature.Summoned ){
								from.SendMessage("Imposs�vel de bondar um summon");
							}
							else if ( creature.Body.IsHuman ){
								from.SendMessage("Imposs�vel de bondar uma pessoa");
							}
							else{	
								
								if( creature.IsBonded == true ){
									from.SendMessage("Este animal ja esta bonded");
								}
								else{
									
									if( from.Skills[SkillName.AnimalTaming].Base  < creature.MinTameSkill ){
										from.SendMessage("Voc� n�o possuiu tamer suficiente para bonda-lo");
									}
									else if( from.Skills[SkillName.AnimalLore].Base  < creature.MinTameSkill ){
											from.SendMessage("Voc� n�o possuiu animal lore suficiente para bonda-lo");
										}
									else{
										try{
											creature.IsBonded = true;
											from.SendMessage("{0} foi bondado com sucesso",creature.Name);
											m_Deed.Delete();
										}
										catch{
											from.SendMessage("Ocorreu um problema ao bondar seu animal. Contacte a Staff");
										}
											
									}
								}
							}							
						}
						else{
							from.SendMessage("Voc� pode bondar somente animais");
						}
					}
					else{
							from.SendMessage("Voc� pode bondar somente animais");
						}
				}
				else{
					from.SendMessage("� necess�rio estar em sua mochila.");
				}			
		}
	}
}
